using Components;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    public class CollisionSystem : SystemBase
    {
        private struct CollisionSystemJob : ICollisionEventsJob
        {
            public BufferFromEntity<CollisionBuffer> Collisions;

            public void Execute(CollisionEvent collisionEvent)
            {
                if (Collisions.Exists(collisionEvent.EntityA))
                {
                    Collisions[collisionEvent.EntityA].Add(new CollisionBuffer()
                    {
                        Entity = collisionEvent.EntityB
                    });
                }

                if (Collisions.Exists(collisionEvent.EntityB))
                {
                    Collisions[collisionEvent.EntityB].Add(new CollisionBuffer()
                    {
                        Entity = collisionEvent.EntityA
                    });
                }
            }
        }

        private struct TriggerSystemJob : ITriggerEventsJob
        {
            public BufferFromEntity<TriggerBuffer> Triggers;

            public void Execute(TriggerEvent triggerEvent)
            {
                if (Triggers.Exists(triggerEvent.EntityA))
                {
                    Triggers[triggerEvent.EntityA].Add(new TriggerBuffer()
                    {
                        Entity = triggerEvent.EntityB
                    });
                }

                if (Triggers.Exists(triggerEvent.EntityB))
                {
                    Triggers[triggerEvent.EntityB].Add(new TriggerBuffer()
                    {
                        Entity = triggerEvent.EntityA
                    });
                }
            }
        }

        protected override void OnUpdate()
        {
            var physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
            var simulation = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;

            Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffer) => { collisionBuffer.Clear(); }).Run();

            var collisionJobHandle = new CollisionSystemJob()
            {
                Collisions = GetBufferFromEntity<CollisionBuffer>()
            }.Schedule(simulation, ref physicsWorld, Dependency);

            collisionJobHandle.Complete();

            var triggerJobHandle = new TriggerSystemJob()
            {
                Triggers = GetBufferFromEntity<TriggerBuffer>()
            }.Schedule(simulation, ref physicsWorld, Dependency);

            triggerJobHandle.Complete();
        }
    }
}
