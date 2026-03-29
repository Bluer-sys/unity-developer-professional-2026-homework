using System;
using Game.Views;
using Modules.Money;
using Zenject;

namespace Game.Presentation
{
    public class MoneyPresenter : IInitializable, IDisposable
    {
        private readonly MoneyView _view;
        private readonly IMoneyStorage _moneyStorage;
        private readonly SignalBus _signalBus;

        public MoneyPresenter(
            MoneyView view,
            IMoneyStorage moneyStorage,
            SignalBus signalBus)
        {
            _view = view;
            _moneyStorage = moneyStorage;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _moneyStorage.OnMoneySpent += SpentMoney;
            _signalBus.Subscribe<OnMoneyEarnedSignal>(EarnMoney);
        }

        public void Dispose()
        {
            _moneyStorage.OnMoneySpent -= SpentMoney;
            _signalBus.Unsubscribe<OnMoneyEarnedSignal>(EarnMoney);
        }

        private void EarnMoney(OnMoneyEarnedSignal signal)
        {
            _view.PlayEarnMoney(signal.From,
                _moneyStorage.Money,
                _moneyStorage.Money - signal.Range);
        }
        
        private void SpentMoney(int newValue, int range) =>
            _view.ChangeMoney(newValue);
    }
}
