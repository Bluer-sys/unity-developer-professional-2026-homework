using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
	[BurstCompile, UpdateAfter(typeof(PurchaseUnitSystem))]
	public partial struct SpawnUnitSystem : ISystem
	{
		private const uint RandomSeed = 17893;

		private Random _random;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<GameState>();
			state.RequireForUpdate<SpawnUnitRequest>();
			_random = new Random(RandomSeed);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			bool isRunning = SystemAPI.GetSingleton<GameState>().IsRunning;
			using var ecb = new EntityCommandBuffer(Allocator.Temp);

			foreach ((
						 DynamicBuffer<SpawnUnitRequest> requests,
						 RefRW<Money> money,
						 RefRO<PlayerBase> playerBase,
						 RefRO<EnemyBaseTarget> enemyBase,
						 RefRO<Team> team,
						 RefRO<UnitSpawnArea> spawnArea,
						 Entity player
					 ) in SystemAPI.Query<
						 DynamicBuffer<SpawnUnitRequest>,
						 RefRW<Money>,
						 RefRO<PlayerBase>,
						 RefRO<EnemyBaseTarget>,
						 RefRO<Team>,
						 RefRO<UnitSpawnArea>>()
					 .WithAll<Player, PurchaseUnitEvent>()
					 .WithEntityAccess())
			{
				for (var i = 0; i < requests.Length; i++)
				{
					SpawnUnitRequest request = requests[i];

					if (!isRunning || !UnitSpawnUseCase.CanSpawn(state.EntityManager,
							request.prefab, request.unitType, team.ValueRO.value, playerBase.ValueRO.value, enemyBase.ValueRO.value))
					{
						money.ValueRW.value += request.price;
						continue;
					}

					LocalTransform baseTransform = SystemAPI.GetComponent<LocalTransform>(playerBase.ValueRO.value);
					LocalTransform transform = SystemAPI.GetComponent<LocalTransform>(request.prefab);
					float2 offset = _random.NextFloat2Direction() * math.sqrt(_random.NextFloat()) * spawnArea.ValueRO.radius;
					transform.Position = baseTransform.Position + spawnArea.ValueRO.offset + new float3(offset.x, 0f, offset.y);

					Entity unit = ecb.Instantiate(request.prefab);
					ecb.SetComponent(unit, transform);
					ecb.SetComponent(unit, team.ValueRO);
					ecb.SetComponent(unit, enemyBase.ValueRO);
					ecb.AppendToBuffer(player, new PurchaseUnitEvent
					{
						unitType = request.unitType,
						unit = unit,
						price = request.price
					});
				}

				requests.Clear();
			}

			if (!ecb.IsEmpty)
				ecb.Playback(state.EntityManager);
		}
	}
}
