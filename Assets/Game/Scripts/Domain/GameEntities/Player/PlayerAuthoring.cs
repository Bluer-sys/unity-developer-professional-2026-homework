using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
	public sealed class PlayerAuthoring : MonoBehaviour
	{
		private const float MinimumIncomePeriod = 0.01f;

		[SerializeField]
		private TeamType _team;

		[SerializeField]
		private Transform _playerBase;

		[SerializeField]
		private Transform _enemyBase;

		[SerializeField]
		private int _initialMoney;

		[SerializeField]
		private float _incomePeriod = 1f;

		[SerializeField]
		private Vector3 _spawnOffset;

		[SerializeField]
		private float _spawnRadius = 2f;

		private sealed class Baker : Baker<PlayerAuthoring>
		{
			public override void Bake(PlayerAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);

				AddComponent<Player>(entity);
				AddComponent(entity, new Team { value = authoring._team });
				AddComponent(entity, new PlayerBase { value = GetEntity(authoring._playerBase, TransformUsageFlags.Dynamic) });
				AddComponent(entity, new EnemyBaseTarget { value = GetEntity(authoring._enemyBase, TransformUsageFlags.Dynamic) });
				AddComponent(entity, new Money { value = math.max(authoring._initialMoney, 0) });
				AddComponent(entity, new MoneyIncome
				{
					time = 0f,
					duration = math.max(authoring._incomePeriod, MinimumIncomePeriod)
				});
				AddComponent(entity, new UnitSpawnArea
				{
					offset = authoring._spawnOffset,
					radius = math.max(authoring._spawnRadius, 0f)
				});
				AddBuffer<PurchaseUnitRequest>(entity);
				AddBuffer<SpawnUnitRequest>(entity);
				AddBuffer<PurchaseUnitEvent>(entity);

				if (authoring._team == TeamType.Red)
					AddComponent<EnemyAI>(entity);
			}
		}
	}
}
