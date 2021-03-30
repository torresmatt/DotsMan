using Components;
using Unity.Entities;

namespace Systems
{
    public class DamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            var entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            var entityCommandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

            Entities.ForEach((DynamicBuffer<CollisionBuffer> collisionBuffer, ref Health health) =>
            {
                for (var i = 0; i < collisionBuffer.Length; i++)
                {
                    if (health.InvincibilityTimer > 0) continue;

                    var entity = collisionBuffer[i].Entity;

                    if (!HasComponent<Damage>(entity)) continue;

                    var damage = GetComponent<Damage>(entity);
                    health.Value -= damage.Value;
                    health.InvincibilityTimer = 1;
                }
            }).Schedule();

            Entities
                .WithNone<Kill>()
                .ForEach((Entity entity, int entityInQueryIndex, ref Health health) =>
                {
                    health.InvincibilityTimer -= deltaTime;

                    if (health.Value <= 0)
                    {
                        entityCommandBuffer.AddComponent(entityInQueryIndex, entity,
                            new Kill {Timer = health.KillTimer});
                    }
                }).Schedule();

            // entityInQueryIndex MUST be named exactly this way
            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Kill kill) =>
            {
                kill.Timer -= deltaTime;
                if (kill.Timer <= 0)
                {
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }
            }).Schedule();

            entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
