using System;
using Modules.Money;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class PlanetPopupPresenter : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private PlanetPopupView _view;
        
        private IMoneyStorage _moneyStorage;

        private IPlanet _shownPlanet;

        [Inject]
        public void Construct(IMoneyStorage moneyStorage) =>
            _moneyStorage = moneyStorage;

        public void Initialize()
        {
            _view.OnUpgrade.AddListener(Upgrade);
            _view.OnClose.AddListener(Hide);
        }

        public void Dispose()
        {
            _view.OnUpgrade.RemoveListener(Upgrade);
            _view.OnClose.RemoveListener(Hide);
        }

        public void Show(IPlanet planet)
        {
            _shownPlanet = planet;
            _shownPlanet.OnIncomeChanged += OnIncomeChanged;
            _shownPlanet.OnPopulationChanged += OnPopulationChanged;
            _shownPlanet.OnUpgraded += OnUpgraded;

            _view.SetVisible(true);
            _view.SetLabelText(_shownPlanet.Name);
            _view.SetSprite(_shownPlanet.GetIcon(true));
            
            OnIncomeChanged(_shownPlanet.MinuteIncome);
            OnPopulationChanged(_shownPlanet.Population);
            OnUpgraded(_shownPlanet.Level);
        }

        private void Hide()
        {
            if (_shownPlanet != null)
            {
                _shownPlanet.OnIncomeChanged -= OnIncomeChanged;
                _shownPlanet.OnPopulationChanged -= OnPopulationChanged;
                _shownPlanet.OnUpgraded -= OnUpgraded;
                _shownPlanet = null;
            }

            _view.SetVisible(false);
        }

        private void Upgrade() => _shownPlanet.Upgrade();

        private void OnPopulationChanged(int value) =>
            _view.SetPopulation($"Population: {value}");

        private void OnIncomeChanged(int value) =>
            _view.SetIncome($"Income: {value} / min");

        private void OnUpgraded(int level)
        {
            int price = _shownPlanet.Price;
            int maxLevel = _shownPlanet.MaxLevel;
            bool isNotMaxLevel = !_shownPlanet.IsMaxLevel;
            bool isEnoughMoney = _moneyStorage.IsEnough(price);

            _view.SetLevel($"Level: {level}/{maxLevel}");
            _view.SetUpgradeLabel(isNotMaxLevel ? "Upgrade" : "MAX LEVEL");
            _view.SetUpgradePrice(price.ToString());
            _view.SetPriceVisible(isNotMaxLevel);
            _view.SetUpgradeButtonInteractable(isNotMaxLevel && isEnoughMoney);
        }
    }
}
