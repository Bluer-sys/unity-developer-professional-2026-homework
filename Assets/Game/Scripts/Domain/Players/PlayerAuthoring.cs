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
				AddComponent(entity, new PlayerBase { value = Entity.Null });
				AddComponent(entity, new EnemyBaseTarget { value = Entity.Null });
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
			}
		}
	}
}
