using UnityEngine;

namespace Game.Money
{
    public class MoneyEarnController : MonoBehaviour
    {
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private EnemyWorld _enemyWorld;

        [SerializeField] private Vector2Int _enemyDeadReward;
        
        private void OnEnable()
        {
            _enemyWorld.OnEnemyDead += OnEnemyDead;
        }

        private void OnDisable()
        {
            _enemyWorld.OnEnemyDead -= OnEnemyDead;
        }

        private void OnEnemyDead()
        {
            var reward = Random.Range(_enemyDeadReward.x, _enemyDeadReward.y + 1);
            
            _moneyStorage.EarnMoney(reward);
        }
    }
}
