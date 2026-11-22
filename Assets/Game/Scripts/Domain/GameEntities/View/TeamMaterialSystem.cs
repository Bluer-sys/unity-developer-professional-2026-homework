using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
	public partial struct TeamMaterialSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = new TeamMaterialJob
			{
				Meshes = SystemAPI.GetComponentLookup<MaterialMeshInfo>()
			}.Schedule(state.Dependency);
		}

		[BurstCompile, WithChangeFilter(typeof(Team), typeof(UnitEquipment))]
		private partial struct TeamMaterialJob : IJobEntity
		{
			public ComponentLookup<MaterialMeshInfo> Meshes;

			private void Execute(in Team team, in DynamicBuffer<UnitEquipment> equipment)
			{
				foreach (UnitEquipment part in equipment)
				{
					Meshes.GetRefRW(part.entity).ValueRW.Material =
						team.value == TeamType.Red ? part.redMaterial : part.blueMaterial;
				}
			}
		}
	}
}
