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

        [Inject]
        public void Construct(IMoneyStorage moneyStorage) =>
            _moneyStorage = moneyStorage;

        public void Initialize()
        {
            _moneyStorage.OnMoneySpent += SpentMoney;
            
            _view.ChangeMoney(_moneyStorage.Money);
        }

        public void Dispose() => 
            _moneyStorage.OnMoneySpent -= SpentMoney;

        public void EarnMoney(Vector3 from, int range) =>
            _view.PlayGather(from, _moneyStorage.Money, _moneyStorage.Money - range);

        private void SpentMoney(int newValue, int range) =>
            _view.ChangeMoney(newValue);
    }
}
