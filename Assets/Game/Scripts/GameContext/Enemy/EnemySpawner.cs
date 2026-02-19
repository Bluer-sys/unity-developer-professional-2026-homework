using System;
using Game.GameObjects;
using UnityEngine;

namespace Game.GameContext
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private EnemyPositionsProvider _positionsProvider;
        [SerializeField] private Player _player;

        public event Action OnEnemyDead;
        
        public int DestroyedEnemies { get; private set; }

        public void Spawn()
        {
            var spawnPosition = _positionsProvider.NextSpawnPosition();
            var destination = _positionsProvider.NextDestination();
            
            var enemy = _enemyPool.Spawn(spawnPosition, Quaternion.identity);

            enemy.SetDestination(destination);
            enemy.SetTarget(_player);
            
            enemy.OnDead += OnEnemyDeadHandler;
        }

        private void OnEnemyDeadHandler(Enemy enemy) 
        {
            enemy.OnDead -= OnEnemyDeadHandler;
            
            enemy.SetTarget(null);
            
            DestroyedEnemies++;
            _enemyPool.Despawn(enemy);

            OnEnemyDead?.Invoke();
        }
    }
}
