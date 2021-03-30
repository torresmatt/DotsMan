using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Collectible : IComponentData
    {
        public float Points;
    }
}
