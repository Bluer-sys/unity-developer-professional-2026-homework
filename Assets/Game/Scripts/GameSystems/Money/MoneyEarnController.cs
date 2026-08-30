using UnityEngine;

namespace Game.Money
{
    public class MoneyEarnController : MonoBehaviour, EnemyWorld.IEnemyDeadListener
    {
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private EnemyWorld _enemyWorld;

        [SerializeField] private Vector2Int _enemyDeadReward;
        
        private void Start()
        {
            _enemyWorld.SetEnemyDeadListener(this);
        }

        void EnemyWorld.IEnemyDeadListener.Invoke()
        {
            var reward = Random.Range(_enemyDeadReward.x, _enemyDeadReward.y + 1);

            _moneyStorage.EarnMoney(reward);
        }
    }
}
