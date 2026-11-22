using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	public static class UnitUseCase
	{
		public static Entity GetEntityByTeam(this EntityQuery query, EntityManager entityManager, TeamType team)
		{
			using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

			foreach (Entity entity in entities)
			{
				if (entityManager.GetComponentData<Team>(entity).value == team)
					return entity;
			}

			return Entity.Null;
		}

		public static bool TryGetItem(this DynamicBuffer<UnitCatalogItem> catalog, UnitType unitType, out UnitCatalogItem item)
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

		public static bool CanSpawn(
				EntityManager entityManager,
				TeamType team,
				Entity playerBase,
				Entity enemyBase
				)
		{
			if (!IsBaseAlive(entityManager, playerBase, out Team ownTeam) || ownTeam.value != team)
				return false;

			if (!IsBaseAlive(entityManager, enemyBase, out Team enemyTeam) || enemyTeam.value == team)
				return false;

			return true;
		}

		private static bool IsBaseAlive(EntityManager entityManager, Entity entity, out Team team)
		{
			team = default;

			if (!entityManager.HasComponent<Base>(entity) ||
				!entityManager.HasComponent<Health>(entity) ||
				!entityManager.HasComponent<Team>(entity) ||
				!entityManager.HasComponent<LocalTransform>(entity))
				return false;

			team = entityManager.GetComponentData<Team>(entity);
			return entityManager.GetComponentData<Health>(entity).IsAlive();
		}
	}
}
