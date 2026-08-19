using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(PresentationSystemGroup))]
	public partial struct TeamColorSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach ((RefRW<URPMaterialPropertyBaseColor> color, RefRO<Team> team)
					 in SystemAPI.Query<RefRW<URPMaterialPropertyBaseColor>, RefRO<Team>>())
				color.ValueRW.Value = team.ValueRO.value switch
				{
					TeamType.Blue => new float4(0.12f, 0.38f, 1f, 1f),
					TeamType.Red => new float4(1f, 0.12f, 0.08f, 1f),
					_ => new float4(0.5f, 0.5f, 0.5f, 1f)
				};
		}
	}
}
