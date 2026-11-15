using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(CleanupSystemGroup))]
	public partial struct PurchaseUnitEventCleanup : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (DynamicBuffer<PurchaseUnitEvent> events in SystemAPI.Query<DynamicBuffer<PurchaseUnitEvent>>())
				events.Clear();
		}
	}
}
