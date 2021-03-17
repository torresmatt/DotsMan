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

            Entities
                .WithAll<Player>()
                .ForEach((ref Movable movable) =>
                {
                    movable.Direction = new float3(horizontalInput, 0, verticalInput);
                }).Schedule();
        }
    }
}
