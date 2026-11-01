using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateInGroup(typeof(CleanupSystemGroup))]
	public partial struct GameOverEventCleanup : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			foreach (EnabledRefRW<GameOverEvent> gameOverEvent in SystemAPI.Query<EnabledRefRW<GameOverEvent>>())
				gameOverEvent.ValueRW = false;
		}
	}
}
