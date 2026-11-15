using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(GameOverSystem))]
	public partial struct MoneyIncomeSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			state.RequireForUpdate<Player>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach ((RefRW<Money> money, RefRW<MoneyIncome> income, RefRO<PlayerBase> playerBase)
				in SystemAPI.Query<RefRW<Money>, RefRW<MoneyIncome>, RefRO<PlayerBase>>().WithAll<Player>())
			{
				if (income.ValueRO.duration <= 0f || playerBase.ValueRO.value == Entity.Null || !SystemAPI.TryGetComponent(playerBase.ValueRO.value, out Health health) || health.IsDead())
					continue;

				income.ValueRW.time += deltaTime;
				var coins = (int)math.floor(income.ValueRO.time / income.ValueRO.duration);

				if (coins <= 0)
					continue;

				money.ValueRW.value += coins;
				income.ValueRW.time -= coins * income.ValueRO.duration;
			}
		}
	}
}
