using System;
using Modules.Money;
using Modules.Planets;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    public class PlanetPopupPresentation : IInitializable
    {
        public ReadOnlyReactiveProperty<string> Label => _label;
        public ReadOnlyReactiveProperty<string> Population => _population;
        public ReadOnlyReactiveProperty<string> Level => _level;
        public ReadOnlyReactiveProperty<string> Income => _income;
        public ReadOnlyReactiveProperty<string> UpgradeLabel => _upgradeLabel;
        public ReadOnlyReactiveProperty<string> UpgradePrice => _upgradePrice;
        public ReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public ReadOnlyReactiveProperty<bool> IsUpgradeButtonInteractable => _isUpgradeButtonInteractable;
        public ReadOnlyReactiveProperty<bool> IsPriceVisible => _isPriceVisible;
        public ReadOnlyReactiveProperty<Sprite> Sprite => _sprite;

        private readonly ReactiveProperty<string> _label = new();
        private readonly ReactiveProperty<string> _population = new();
        private readonly ReactiveProperty<string> _level = new();
        private readonly ReactiveProperty<string> _income = new();
        private readonly ReactiveProperty<string> _upgradeLabel = new();
        private readonly ReactiveProperty<string> _upgradePrice = new();
        private readonly ReactiveProperty<bool> _isVisible = new();
        private readonly ReactiveProperty<bool> _isUpgradeButtonInteractable = new();
        private readonly ReactiveProperty<bool> _isPriceVisible = new();
        private readonly ReactiveProperty<Sprite> _sprite = new();
        
        private readonly IMoneyStorage _moneyStorage;

        private IPlanet _shownPlanet;

        public PlanetPopupPresentation(IMoneyStorage moneyStorage) 
        {
            _moneyStorage = moneyStorage;
        }

        public void Initialize()
        {
            Hide();
        }

        public void Show(IPlanet planet)
        {
            _shownPlanet = planet;
            _shownPlanet.OnIncomeChanged += OnIncomeChanged;
            _shownPlanet.OnPopulationChanged += OnPopulationChanged;
            _shownPlanet.OnUpgraded += OnUpgraded;
            
            _isVisible.Value = true;
            _label.Value = _shownPlanet.Name;
            _sprite.Value = _shownPlanet.GetIcon(true);
            
            OnIncomeChanged(_shownPlanet.MinuteIncome);
            OnPopulationChanged(_shownPlanet.Population);
            OnUpgraded(_shownPlanet.Level);
        }

        public void Hide()
        {
            if (_shownPlanet != null)
            {
                _shownPlanet.OnIncomeChanged -= OnIncomeChanged;
                _shownPlanet.OnPopulationChanged -= OnPopulationChanged;
                _shownPlanet.OnUpgraded -= OnUpgraded;
                _shownPlanet = null;
            }
            
            _isVisible.Value = false;
        }

        public void Upgrade()
        {
            _shownPlanet.Upgrade();
        }

        private void OnUpgraded(int level)
        {
            bool isNotMaxLevel = !_shownPlanet.IsMaxLevel;
            bool isEnoughMoney = _moneyStorage.IsEnough(_shownPlanet.Price);
            int maxLevel = _shownPlanet.MaxLevel;

            _level.Value = $"Level: {level}/{maxLevel}";
            _upgradeLabel.Value = isNotMaxLevel ? "Upgrade" : "MAX LEVEL";
            _upgradePrice.Value = level.ToString();
            _isPriceVisible.Value = isNotMaxLevel;
            _isUpgradeButtonInteractable.Value = isNotMaxLevel && isEnoughMoney;
        }

        private void OnPopulationChanged(int value)
        {
            _population.Value = $"Population: {value}";
        }

        private void OnIncomeChanged(int value)
        {
            _income.Value = $"Income: {value} / min";
        }
    }
}
