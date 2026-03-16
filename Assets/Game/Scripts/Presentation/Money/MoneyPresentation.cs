using System;
using Game.Presentation.Signals;
using Modules.Money;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Presentation.Money
{
    public class MoneyPresentation : IInitializable, IDisposable
    {
        public int Money => _moneyStorage.Money;
        public Observable<int> OnMoneySpent => _onMoneySpent;
        public Observable<(Vector3 from, int cur, int prev)> OnMoneyEarned => _onMoneyEarned;

        private readonly ReactiveCommand<int> _onMoneySpent = new();
        private readonly ReactiveCommand<(Vector3, int, int)> _onMoneyEarned = new();

        private readonly IMoneyStorage _moneyStorage;
        private readonly SignalBus _signalBus;

        public MoneyPresentation(
            IMoneyStorage moneyStorage,
            SignalBus signalBus)
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
            _onMoneyEarned.Execute(
                (signal.From,
                 _moneyStorage.Money,
                 _moneyStorage.Money - signal.Range));
        }
        
        private void SpentMoney(int newValue, int range)
        {
            _onMoneySpent.Execute(newValue);
        }
    }
}
