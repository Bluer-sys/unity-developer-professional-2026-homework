using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(CleanupSystemGroup))]
	public partial struct TakeDamageEventCleanup : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (DynamicBuffer<TakeDamageEvent> events in SystemAPI.Query<DynamicBuffer<TakeDamageEvent>>())
				events.Clear();
		}
	}
}
