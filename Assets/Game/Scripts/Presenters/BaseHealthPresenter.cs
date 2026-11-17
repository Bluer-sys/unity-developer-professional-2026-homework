using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class BaseHealthPresenter : MonoBehaviour
	{
		[SerializeField] private TeamType _team;
		[SerializeField] private HealthView _view;

		private World _world;
		private EntityQuery _basesQuery;
		private int _health;
		private int _maxHealth;

		private void OnEnable()
		{
			SetValues(0, 0);
		}

		private void OnDisable()
		{
			Disconnect();
		}

		private void LateUpdate()
		{
			if (!TryConnect())
			{
				SetValues(0, 0);
				return;
			}

			EntityManager entityManager = _world.EntityManager;
			Entity playerBase = _basesQuery.GetEntityByTeam(entityManager, _team);

			if (playerBase == Entity.Null)
			{
				SetValues(0, Mathf.Max(_maxHealth, 0));
				return;
			}

			int maxHealth = Mathf.Max(entityManager.GetComponentData<MaxHealth>(playerBase).value, 0);
			int health = Mathf.Clamp(entityManager.GetComponentData<Health>(playerBase).value, 0, maxHealth);
			SetValues(health, maxHealth);
		}

		private bool TryConnect()
		{
			World world = World.DefaultGameObjectInjectionWorld;

			if (_world == world && 
				world != null && 
				world.IsCreated)
				return true;

			Disconnect();
			SetValues(0, 0);

			if (world == null || !world.IsCreated)
				return false;

			_world = world;
			_basesQuery = world.EntityManager.CreateEntityQuery(
				ComponentType.ReadOnly<Base>(),
				ComponentType.ReadOnly<Team>(),
				ComponentType.ReadOnly<Health>(),
				ComponentType.ReadOnly<MaxHealth>());
			return true;
		}

		private void Disconnect()
		{
			if (_world != null && _world.IsCreated)
				_basesQuery.Dispose();

			_world = null;
			_basesQuery = default;
		}

		private void SetValues(int health, int maxHealth)
		{
			if (_health == health && _maxHealth == maxHealth)
				return;

			_health = health;
			_maxHealth = maxHealth;
			_view.HealthText = $"{health}/{maxHealth}";
			_view.HealthProgress = (float)health / maxHealth;
		}
	}
}
