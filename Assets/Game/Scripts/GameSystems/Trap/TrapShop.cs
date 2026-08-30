using System;
using System.Linq;
using Fusion;
using Game.Money;
using UnityEngine;

namespace Game
{
    public class TrapShop : SimulationBehaviour
    {
        [SerializeField] private Data[] _data;
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private TrapWorld _trapWorld;

        public bool TryBuy(TrapType type, NetworkObject buyer)
        {
            int price = _data.FirstOrDefault(p => p.Type == type).Price;

            if (!_moneyStorage.TrySpendMoney(price)) 
                return false;
            
            _trapWorld.Spawn(type, buyer.GetBehaviour<Hero>().transform.position);
            return true;
        }

        [Serializable]
        public struct Data
        {
            public TrapType Type;
            public int Price;
        }
    }
}
