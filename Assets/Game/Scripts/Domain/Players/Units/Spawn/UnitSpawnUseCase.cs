using Unity.Entities;
using Unity.Transforms;

namespace Game
{
	public static class UnitSpawnUseCase
	{
		public static bool CanSpawn(
				EntityManager entityManager,
				Entity prefab,
				CombatUnitType unitType,
				TeamType team,
				Entity playerBase,
				Entity enemyBase
				)
		{
			if (team != TeamType.Blue && team != TeamType.Red)
				return false;

			if (!IsBaseAlive(entityManager, playerBase, out Team ownTeam) || ownTeam.value != team)
				return false;

			if (!IsBaseAlive(entityManager, enemyBase, out Team enemyTeam) || enemyTeam.value == team)
				return false;

			if (!entityManager.HasComponent<Prefab>(prefab) ||
				!entityManager.HasComponent<Unit>(prefab) ||
				!entityManager.HasComponent<LocalTransform>(prefab) ||
				!entityManager.HasComponent<Team>(prefab) ||
				!entityManager.HasComponent<EnemyBaseTarget>(prefab))
				return false;

			return unitType switch
			{
				CombatUnitType.Swordsman => entityManager.HasComponent<Swordsman>(prefab),
				CombatUnitType.Archer => entityManager.HasComponent<Archer>(prefab),
				_ => false
			};
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
