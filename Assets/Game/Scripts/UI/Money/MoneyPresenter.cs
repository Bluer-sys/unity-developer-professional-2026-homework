using System;
using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class MoneyPresenter : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private MoneyView _view;
        
        private IMoneyStorage _moneyStorage;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IMoneyStorage moneyStorage, SignalBus signalBus)
        {
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
            _view.PlayGather(signal.From,
                _moneyStorage.Money,
                _moneyStorage.Money - signal.Range);
        }
        
        private void SpentMoney(int newValue, int range) =>
            _view.ChangeMoney(newValue);
    }
}
