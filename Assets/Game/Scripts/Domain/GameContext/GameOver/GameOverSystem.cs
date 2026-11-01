using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(HealthEmptySystem)), UpdateBefore(typeof(DeathCooldownSystem))]
	public partial struct GameOverSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency.Complete();

			GameState gameState = SystemAPI.GetSingleton<GameState>();

			if (!gameState.IsRunning)
				return;

			bool playerBaseDestroyed = false;
			bool enemyBaseDestroyed = false;

			foreach (RefRO<Team> team in SystemAPI.Query<RefRO<Team>>().WithAll<Base, DeathEvent>())
			{
				if (team.ValueRO.value == TeamType.Blue)
					playerBaseDestroyed = true;
				else if (team.ValueRO.value == TeamType.Red)
					enemyBaseDestroyed = true;
			}

			if (!playerBaseDestroyed && !enemyBaseDestroyed)
				return;

			gameState.value = playerBaseDestroyed ? GameStatus.Loss : GameStatus.Win;
			SystemAPI.SetSingleton(gameState);

			Entity gameContext = SystemAPI.GetSingletonEntity<GameState>();
			SystemAPI.SetComponentEnabled<GameOverEvent>(gameContext, true);
		}
	}
}
