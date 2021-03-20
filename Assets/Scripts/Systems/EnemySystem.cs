using System;
using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class EnemySystem : SystemBase
    {
        private readonly Random _random = new Random((uint) Environment.TickCount);

        protected override void OnUpdate()
        {
            var rayCaster = new MovementRaycast()
            {
                PhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld
            };

            var random = _random;
            random.InitState((uint) Environment.TickCount);

            Entities.ForEach((ref Movable movable, ref Enemy enemy, in Translation translation,
                in PhysicsCollider collider) =>
            {
                var previousCell = enemy.PreviousCell;
                var currentCell = translation.Value;
                var distance = math.distance(currentCell, previousCell);

                if (distance <= 0.9f) return;

                enemy.PreviousCell = math.round(currentCell);

                var validDirections = new NativeList<float3>(Allocator.Temp);

                var newDirection = Float3Extensions.Forward;

                // TODO: This screams to be abstracted to a method
                var currentDirection = movable.Direction;
                var hitSomething = rayCaster.CheckRay(currentCell, newDirection, currentDirection, collider);
                if (!hitSomething) validDirections.Add(newDirection);

                newDirection = Float3Extensions.Back;
                hitSomething = rayCaster.CheckRay(currentCell, newDirection, currentDirection, collider);
                if (!hitSomething) validDirections.Add(newDirection);

                newDirection = Float3Extensions.Left;
                hitSomething = rayCaster.CheckRay(currentCell, newDirection, currentDirection, collider);
                if (!hitSomething) validDirections.Add(newDirection);

                newDirection = Float3Extensions.Right;
                hitSomething = rayCaster.CheckRay(currentCell, newDirection, currentDirection, collider);
                if (!hitSomething) validDirections.Add(newDirection);

                var randomIndex = random.NextInt(validDirections.Length);
                movable.Direction = validDirections[randomIndex];

                validDirections.Dispose();
            }).Schedule();
        }

        private struct MovementRaycast
        {
            [ReadOnly] public PhysicsWorld PhysicsWorld;

            public bool CheckRay(float3 position, float3 direction, float3 currentDirection, PhysicsCollider collider)
            {
                if (direction.Equals(-currentDirection)) return true;

                var ray = new RaycastInput()
                {
                    Start = position,
                    End = position + (direction * 0.9f),
                    Filter = new CollisionFilter()
                    {
                        GroupIndex = 0,
                        BelongsTo = collider.Value.Value.Filter.BelongsTo,
                        CollidesWith = collider.Value.Value.Filter.CollidesWith
                    }
                };

                return PhysicsWorld.CastRay(ray);
            }
        }
    }
}
