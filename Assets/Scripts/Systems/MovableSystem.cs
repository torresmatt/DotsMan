using Components;
using Unity.Entities;
using Unity.Physics;

namespace Systems
{
    public class MovableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach(
                    (ref PhysicsVelocity physicsVelocity, in Movable movableComponent) =>
                    {
                        var direction = movableComponent.Direction;
                        var speed = movableComponent.Speed;
                        var downVelocity = physicsVelocity.Linear.y;

                        var step = direction * speed;
                        step.y = downVelocity;

                        physicsVelocity.Linear = step;
                    })
                .Schedule();
        }
    }
}
