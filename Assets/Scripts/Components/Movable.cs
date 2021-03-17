using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Movable : IComponentData
    {
        public float Speed;
        [HideInInspector] public float3 Direction;
    }
}
