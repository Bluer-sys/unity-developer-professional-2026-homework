using System;
using Game.Views;
using Modules.Money;
using Modules.Planets;
using Zenject;

namespace Game.Presentation
{
    public class PlanetPopupPresenter : IInitializable, IDisposable
    {
        private readonly PlanetPopupView _view;
        private readonly IMoneyStorage _moneyStorage;
        private readonly SignalBus _signalBus;

        private IPlanet _shownPlanet;

        public PlanetPopupPresenter(PlanetPopupView view, IMoneyStorage moneyStorage, SignalBus signalBus)
        {
            _view = view;
            _moneyStorage = moneyStorage;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<OnPlanetPopupRequestedSignal>(Show);
            
            _view.OnUpgrade.AddListener(Upgrade);
            _view.OnClose.AddListener(Hide);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OnPlanetPopupRequestedSignal>(Show);
            
            _view.OnUpgrade.RemoveListener(Upgrade);
            _view.OnClose.RemoveListener(Hide);
        }

        private void Show(OnPlanetPopupRequestedSignal signal)
        {
            _shownPlanet = signal.Planet;
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
            bool isNotMaxLevel = !_shownPlanet.IsMaxLevel;
            bool isEnoughMoney = _moneyStorage.IsEnough(_shownPlanet.Price);
            int maxLevel = _shownPlanet.MaxLevel;

            _view.SetLevel($"Level: {level}/{maxLevel}");
            _view.SetUpgradeLabel(isNotMaxLevel ? "Upgrade" : "MAX LEVEL");
            _view.SetUpgradePrice(level.ToString());
            _view.SetPriceVisible(isNotMaxLevel);
            _view.SetUpgradeButtonInteractable(isNotMaxLevel && isEnoughMoney);
        }
    }
}
