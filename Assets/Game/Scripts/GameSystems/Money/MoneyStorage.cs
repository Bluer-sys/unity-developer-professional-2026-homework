using System;
using Fusion;
using UnityEngine;

namespace Game.Money
{
    public class MoneyStorage : NetworkBehaviour
    {
        public event Action<int> OnStateChanged; 
        
        [SerializeField] private int _startMoney;
        
        [Networked, OnChangedRender(nameof(InvokeStateChanged))]
        public int Current { get; private set; }

        public override void Spawned()
        {
            Current = _startMoney;
            InvokeStateChanged();
        }

        public void EarnMoney(int amount)
        {
            Current += amount;
        }

        public bool TrySpendMoney(int amount)
        {
            if (Current < amount)
                return false;
            
            Current -= amount;
            return true;
        }
        
        private void InvokeStateChanged()
        {
            OnStateChanged?.Invoke(Current);
        }
    }
}
