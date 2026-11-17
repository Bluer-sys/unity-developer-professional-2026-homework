using TMPro;
using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class MoneyPresenter : MonoBehaviour
	{
		[SerializeField] private TeamType _team;
		[SerializeField] private TMP_Text _view;

		private World _world;
		private EntityQuery _playersQuery;
		private int _money = -1;

		private void OnEnable()
		{
			SetMoney(0);
		}

		private void OnDisable()
		{
			Disconnect();
		}

		private void LateUpdate()
		{
			if (!TryConnect())
			{
				SetMoney(0);
				return;
			}

			EntityManager entityManager = _world.EntityManager;
			Entity player = _playersQuery.GetEntityByTeam(entityManager, _team);
			SetMoney(player != Entity.Null ? entityManager.GetComponentData<Money>(player).value : 0);
		}

		private bool TryConnect()
		{
			World world = World.DefaultGameObjectInjectionWorld;

			if (_world == world && world != null && world.IsCreated)
				return true;

			Disconnect();

			if (world == null || !world.IsCreated)
				return false;

			_world = world;
			_playersQuery = world.EntityManager.CreateEntityQuery(
				ComponentType.ReadOnly<Player>(),
				ComponentType.ReadOnly<Team>(),
				ComponentType.ReadOnly<Money>());
			
			return true;
		}

		private void Disconnect()
		{
			if (_world != null && _world.IsCreated)
				_playersQuery.Dispose();

			_world = null;
			_playersQuery = default;
		}

		private void SetMoney(int money)
		{
			if (_money == money)
				return;

			_money = money;
			_view.text = money.ToString();
		}
	}
}
