using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class CardsPresenter : MonoBehaviour
	{
		[SerializeField] private UnitCardsCatalog _catalog;
		[SerializeField] private UnitCardPresenter _cardPrefab;
		[SerializeField] private Transform _cardsRoot;

		private readonly List<UnitCardPresenter> _cards = new();
		private World _world;
		private EntityQuery _playersQuery;
		private EntityQuery _gameStateQuery;
		private EntityQuery _catalogQuery;

		private void Awake()
		{
			var unitTypes = new HashSet<UnitType>();

			foreach (UnitCardConfig config in _catalog.Cards)
			{
				if (!unitTypes.Add(config.UnitType))
					continue;

				UnitCardPresenter card = Instantiate(_cardPrefab, _cardsRoot, false);
				card.Initialize(config, RequestPurchase);
				_cards.Add(card);
			}
		}

		private void OnEnable()
		{
			foreach (UnitCardPresenter card in _cards)
				card.enabled = true;

			ResetViews();
		}

		private void OnDisable()
		{
			foreach (UnitCardPresenter card in _cards)
			{
				if (card != null)
					card.enabled = false;
			}

			Disconnect();
		}

		private void OnDestroy()
		{
			foreach (UnitCardPresenter card in _cards)
			{
				if (card != null)
					Destroy(card.gameObject);
			}
		}

		private void LateUpdate()
		{
			if (!TryConnect() || _gameStateQuery.CalculateEntityCount() != 1)
			{
				ResetViews();
				return;
			}

			EntityManager entityManager = _world.EntityManager;
			Entity player = _playersQuery.GetEntityByTeam(entityManager, TeamType.Blue);
			int money = player != Entity.Null ? entityManager.GetComponentData<Money>(player).value : 0;
			bool isRunning = _gameStateQuery.GetSingleton<GameState>().IsRunning;
			bool hasCatalog = _catalogQuery.CalculateEntityCount() == 1;
			
			DynamicBuffer<UnitCatalogItem> catalog = hasCatalog
				? _catalogQuery.GetSingletonBuffer<UnitCatalogItem>(true)
				: default;
			
			Entity playerBase = player != Entity.Null
				? entityManager.GetComponentData<PlayerBase>(player).value
				: Entity.Null;
			
			Entity targetBase = player != Entity.Null
				? entityManager.GetComponentData<EnemyBaseTarget>(player).value
				: Entity.Null;

			foreach (UnitCardPresenter card in _cards)
			{
				if (!hasCatalog || !catalog.TryGetItem(card.UnitType, out UnitCatalogItem item))
				{
					card.Refresh(money, card.ConfiguredPrice, false, false);
					continue;
				}

				bool canPurchase = isRunning && player != Entity.Null && money >= item.price &&
					UnitUseCase.CanSpawn(entityManager, TeamType.Blue, playerBase, targetBase);

				card.Refresh(money, item.price, true, canPurchase);
			}
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
			EntityManager entityManager = world.EntityManager;
			_playersQuery = entityManager.CreateEntityQuery(
				ComponentType.ReadOnly<Player>(),
				ComponentType.ReadOnly<Team>(),
				ComponentType.ReadOnly<PlayerBase>(),
				ComponentType.ReadOnly<EnemyBaseTarget>(),
				ComponentType.ReadOnly<Money>(),
				ComponentType.ReadWrite<PurchaseUnitRequest>());
			
			_gameStateQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<GameState>());
			_catalogQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<UnitCatalogItem>());
			return true;
		}

		private void Disconnect()
		{
			if (_world != null && _world.IsCreated)
			{
				_playersQuery.Dispose();
				_gameStateQuery.Dispose();
				_catalogQuery.Dispose();
			}

			_world = null;
			_playersQuery = default;
			_gameStateQuery = default;
			_catalogQuery = default;
		}

		private void RequestPurchase(UnitType unitType)
		{
			if (!isActiveAndEnabled || !TryConnect() || _gameStateQuery.CalculateEntityCount() != 1)
				return;

			if (!_gameStateQuery.GetSingleton<GameState>().IsRunning)
				return;

			EntityManager entityManager = _world.EntityManager;
			Entity player = _playersQuery.GetEntityByTeam(entityManager, TeamType.Blue);

			if (player == Entity.Null)
				return;

			DynamicBuffer<PurchaseUnitRequest> requests = entityManager.GetBuffer<PurchaseUnitRequest>(player);
			requests.Add(new PurchaseUnitRequest { unitType = unitType });
		}

		private void ResetViews()
		{
			foreach (UnitCardPresenter card in _cards)
				card.Refresh(0, card.ConfiguredPrice, true, false);
		}
	}
}
