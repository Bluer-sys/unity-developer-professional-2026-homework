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
			foreach (EnabledRefRW<AttackStartedEvent> attack in SystemAPI.Query<EnabledRefRW<AttackStartedEvent>>())
				attack.ValueRW = false;

			foreach (EnabledRefRW<FireEvent> fireEvent in SystemAPI.Query<EnabledRefRW<FireEvent>>())
				fireEvent.ValueRW = false;
		}
	}
}
