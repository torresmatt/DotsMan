using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct PowerPill : IComponentData
    {
        public float PillTimer;
        public float DamageBonus;
    }
}
