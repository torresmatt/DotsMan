using Components;
using Unity.Entities;

namespace Systems
{
    public class CollectionSystem : SystemBase
    {
        private EntityCommandBuffer _entityCommandBuffer;

        protected override void OnUpdate()
        {
            _entityCommandBuffer =
                World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

            Entities
                .WithAll<Player>()
                .ForEach((Entity playerEntity, DynamicBuffer<TriggerBuffer> triggerBuffer) =>
                {
                    foreach (var t in triggerBuffer)
                    {
                        var entity = t.Entity;

                        if (HasComponent<Collectible>(entity) && !HasComponent<Kill>(entity))
                        {
                            _entityCommandBuffer.AddComponent(entity, new Kill {Timer = 0});
                        }

                        if (HasComponent<PowerPill>(entity) && !HasComponent<Kill>(entity))
                        {
                            var powerPill = GetComponent<PowerPill>(entity);

                            _entityCommandBuffer.AddComponent(playerEntity, powerPill);
                            _entityCommandBuffer.AddComponent(entity, new Kill {Timer = 0});

                            _entityCommandBuffer.AddComponent(playerEntity, new Damage {Value = powerPill.DamageBonus});
                        }
                    }
                })
                .WithStructuralChanges()
                .Run();
        }
    }
}
