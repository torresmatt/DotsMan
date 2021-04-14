using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Systems
{
    public class PlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var deltaTime = Time.DeltaTime;

            var entityCommandBuffer =
                World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

            Entities
                .WithAll<Player>()
                .ForEach((ref Movable movable) =>
                {
                    movable.Direction = new float3(horizontalInput, 0, verticalInput);
                }).Schedule();

            Entities
                .WithAll<Player>()
                .ForEach((Entity entity, ref Health health, ref PowerPill powerPill, ref Damage damage) =>
                {
                    damage.Value = 100;
                    powerPill.PillTimer -= deltaTime;
                    health.InvincibilityTimer = powerPill.PillTimer;

                    if (powerPill.PillTimer > 0) return;

                    entityCommandBuffer.RemoveComponent<PowerPill>(entity);
                    entityCommandBuffer.RemoveComponent<Damage>(entity);
                }).Run();
        }
    }
}
