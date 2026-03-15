using System;
using Modules.Money;
using R3;
using Zenject;

namespace Game.Presentation
{
    public class MoneyPanelPresentation : IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<int> Money => _money;

        private readonly ReactiveProperty<int> _money = new();
        
        private readonly IMoneyStorage _moneyStorage;

        public MoneyPanelPresentation(IMoneyStorage moneyStorage) =>
            _moneyStorage = moneyStorage;

        public void Initialize()
        {
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            
            _money.Value = _moneyStorage.Money;
        }

        public void Dispose() =>
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;

        public string Formate(int value) => value.ToString();
        
        private void OnMoneyChanged(int newValue, int prevValue) =>
            _money.Value = newValue;
    }
}
