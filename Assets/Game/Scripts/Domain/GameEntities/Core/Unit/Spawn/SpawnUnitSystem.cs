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
				bool canSpawn = isRunning && UnitUseCase.CanSpawn(state.EntityManager,
					team.ValueRO.value, playerBase.ValueRO.value, enemyBase.ValueRO.value);

				for (var i = 0; i < requests.Length; i++)
				{
					SpawnUnitRequest request = requests[i];

					if (!canSpawn)
					{
						money.ValueRW.value += request.price;
						continue;
					}

					Entity unit = SpawnUnit(state.EntityManager, ecb, request.prefab,
						playerBase.ValueRO.value, team.ValueRO, enemyBase.ValueRO, spawnArea.ValueRO);
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

		private Entity SpawnUnit(
				EntityManager entityManager,
				EntityCommandBuffer ecb,
				Entity prefab,
				Entity playerBase,
				Team team,
				EnemyBaseTarget enemyBase,
				UnitSpawnArea spawnArea
			)
		{
			LocalTransform baseTransform = entityManager.GetComponentData<LocalTransform>(playerBase);
			LocalTransform transform = entityManager.GetComponentData<LocalTransform>(prefab);
			float2 offset = _random.NextFloat2Direction() * math.sqrt(_random.NextFloat()) * spawnArea.radius;
			transform.Position = baseTransform.Position + spawnArea.offset + new float3(offset.x, 0f, offset.y);

			Entity unit = ecb.Instantiate(prefab);
			ecb.SetComponent(unit, transform);
			ecb.SetComponent(unit, team);
			ecb.SetComponent(unit, enemyBase);
			return unit;
		}
	}
}
