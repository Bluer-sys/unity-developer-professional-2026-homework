using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(SpawnUnitSystem))]
	public partial struct EnemyAISystem : ISystem
	{
		private const uint RandomSeed = 96271;

		private Random _random;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			state.RequireForUpdate<EnemyAI>();
			state.RequireForUpdate<UnitCatalogItem>();
			_random = new Random(RandomSeed);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			if (!SystemAPI.GetSingleton<GameState>().IsRunning)
				return;

			DynamicBuffer<UnitCatalogItem> catalog = SystemAPI.GetSingletonBuffer<UnitCatalogItem>(true);

			foreach ((
						 RefRW<EnemyAI> enemyAI,
						 RefRO<Money> money,
						 DynamicBuffer<PurchaseUnitRequest> requests,
						 DynamicBuffer<PurchaseUnitEvent> events,
						 RefRO<PlayerBase> playerBase,
						 RefRO<EnemyBaseTarget> enemyBase,
						 RefRO<Team> team
					 ) in SystemAPI.Query<
						 RefRW<EnemyAI>,
						 RefRO<Money>,
						 DynamicBuffer<PurchaseUnitRequest>,
						 DynamicBuffer<PurchaseUnitEvent>,
						 RefRO<PlayerBase>,
						 RefRO<EnemyBaseTarget>,
						 RefRO<Team>>()
					 .WithAll<Player, UnitSpawnArea, SpawnUnitRequest>())
			{
				if (enemyAI.ValueRO.isWaitingForPurchase)
				{
					if (events.Length > 0)
						enemyAI.ValueRW.hasSelectedUnit = false;
					else if (requests.Length > 0)
						continue;

					enemyAI.ValueRW.isWaitingForPurchase = false;
				}

				if (!enemyAI.ValueRO.hasSelectedUnit)
				{
					enemyAI.ValueRW.unitIndex = _random.NextInt(catalog.Length);
					enemyAI.ValueRW.hasSelectedUnit = true;
				}

				UnitCatalogItem item = catalog[enemyAI.ValueRO.unitIndex];

				if (requests.Length > 0 || money.ValueRO.value < item.price)
					continue;

				if (!UnitUseCase.CanSpawn(state.EntityManager,
						team.ValueRO.value, playerBase.ValueRO.value, enemyBase.ValueRO.value))
					continue;

				requests.Add(new PurchaseUnitRequest { unitType = item.unitType });
				enemyAI.ValueRW.isWaitingForPurchase = true;
			}
		}
	}
}
