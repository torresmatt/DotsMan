using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Health : IComponentData
    {
        public float Value;
        public float InvincibilityTimer;
        public float KillTimer;
    }
}
