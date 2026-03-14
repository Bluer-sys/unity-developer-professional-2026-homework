using System;
using Modules.Money;
using R3;
using Zenject;

namespace Game.Presentation
{
    public class MoneyPanelPresentation : IInitializable, IDisposable
    {
        public ReadOnlyReactiveProperty<string> Money => _money;

        private readonly ReactiveProperty<string> _money = new();
        
        private readonly IMoneyStorage _moneyStorage;

        public MoneyPanelPresentation(IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
        }
        
        public void Initialize()
        {
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            
            _money.Value = _moneyStorage.Money.ToString();
        }

        public void Dispose()
        {
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged; 
        }

        private void OnMoneyChanged(int newValue, int prevValue)
        {
            _money.Value = newValue.ToString();
        }
    }
}
