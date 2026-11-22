using System;
using UnityEngine;

namespace Game
{
	public sealed class UnitCardPresenter : MonoBehaviour
	{
		[SerializeField] private UnitCardView _view;

		private Action<UnitType> _purchase;
		private bool _canPurchase;
		private int _money = -1;
		private int _price = -1;

		public UnitType UnitType { get; private set; }
		public int ConfiguredPrice { get; private set; }

		public void Initialize(UnitCardConfig config, Action<UnitType> purchase)
		{
			_purchase = purchase;
			UnitType = config.UnitType;
			ConfiguredPrice = config.Price;
			_canPurchase = false;
			_money = -1;
			_price = -1;

			_view.SetName(config.Name);
			_view.SetIcon(config.Icon);
			_view.SetEnabled(false);
		}

		private void OnEnable()
		{
			_view.OnClicked += OnClicked;
			_canPurchase = false;
			_view.SetEnabled(false);
		}

		private void OnDisable()
		{
			_canPurchase = false;

			_view.OnClicked -= OnClicked;
			_view.SetEnabled(false);
		}

		public void Refresh(int money, int price, bool isVisible, bool canPurchase)
		{
			if (gameObject.activeSelf != isVisible)
				gameObject.SetActive(isVisible);

			if (_money != money || _price != price)
			{
				_money = money;
				_price = price;
				_view.SetProgressCaption($"{money}/{price}");
				_view.SetProgress(Mathf.Clamp01((float)money / price));
			}

			canPurchase &= isVisible && isActiveAndEnabled;

			if (_canPurchase != canPurchase)
			{
				_canPurchase = canPurchase;
				_view.SetEnabled(canPurchase);
			}
		}

		private void OnClicked()
		{
			if (isActiveAndEnabled && _canPurchase)
				_purchase(UnitType);
		}
	}
}
