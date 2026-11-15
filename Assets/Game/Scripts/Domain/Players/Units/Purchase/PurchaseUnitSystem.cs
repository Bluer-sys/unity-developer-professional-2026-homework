using Unity.Burst;
using Unity.Entities;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(MoneyIncomeSystem))]
	public partial struct PurchaseUnitSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			state.RequireForUpdate<UnitCatalogItem>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			bool isRunning = SystemAPI.GetSingleton<GameState>().IsRunning;
			DynamicBuffer<UnitCatalogItem> catalog = SystemAPI.GetSingletonBuffer<UnitCatalogItem>(true);

			foreach ((
						 DynamicBuffer<PurchaseUnitRequest> requests,
						 DynamicBuffer<SpawnUnitRequest> spawnRequests,
						 RefRW<Money> money,
						 RefRO<PlayerBase> playerBase,
						 RefRO<EnemyBaseTarget> enemyBase,
						 RefRO<Team> team
					 ) in SystemAPI.Query<
						 DynamicBuffer<PurchaseUnitRequest>,
						 DynamicBuffer<SpawnUnitRequest>,
						 RefRW<Money>,
						 RefRO<PlayerBase>,
						 RefRO<EnemyBaseTarget>,
						 RefRO<Team>>()
					 .WithAll<Player, UnitSpawnArea, PurchaseUnitEvent>())
			{
				for (var i = 0; i < requests.Length; i++)
				{
					if (!isRunning || !TryGetItem(catalog, requests[i].unitType, out UnitCatalogItem item))
						continue;

					if (item.price < 0 || money.ValueRO.value < item.price)
						continue;

					if (!UnitSpawnUseCase.CanSpawn(state.EntityManager,
							item.prefab, item.unitType, team.ValueRO.value, playerBase.ValueRO.value, enemyBase.ValueRO.value))
						continue;

					money.ValueRW.value -= item.price;
					spawnRequests.Add(new SpawnUnitRequest
					{
						unitType = item.unitType,
						prefab = item.prefab,
						price = item.price
					});
				}

				requests.Clear();
			}
		}

		private static bool TryGetItem(DynamicBuffer<UnitCatalogItem> catalog, CombatUnitType unitType, out UnitCatalogItem item)
		{
			for (var i = 0; i < catalog.Length; i++)
			{
				if (catalog[i].unitType != unitType)
					continue;

				item = catalog[i];
				return true;
			}

			item = default;
			return false;
		}
	}
}
