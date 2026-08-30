using UnityEngine;

namespace Game.Money
{
    public class MoneyEarnController : MonoBehaviour, EnemyWorld.IEnemyDeadAction
    {
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private EnemyWorld _enemyWorld;

        [SerializeField] private Vector2Int _enemyDeadReward;
        
        private void Start()
        {
            _enemyWorld.SetEnemyDeadAction(this);
        }

        void EnemyWorld.IEnemyDeadAction.Invoke()
        {
            var reward = Random.Range(_enemyDeadReward.x, _enemyDeadReward.y + 1);

            _moneyStorage.EarnMoney(reward);
        }
    }
}
