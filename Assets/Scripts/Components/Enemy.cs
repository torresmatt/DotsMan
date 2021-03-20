using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Enemy : IComponentData
    {
        [HideInInspector] public float3 PreviousCell;
    }
}
