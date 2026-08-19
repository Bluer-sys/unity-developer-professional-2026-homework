using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(CleanupSystemGroup))]
	public partial struct FireEventCleanup : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (EnabledRefRW<FireEvent> fireEvent in SystemAPI.Query<EnabledRefRW<FireEvent>>())
				fireEvent.ValueRW = false;
		}
	}
}
