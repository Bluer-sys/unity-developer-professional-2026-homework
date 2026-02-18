using System;
using Game.GameObjects.Ship;
using Game.Utils;
using UnityEngine;

namespace Game.GameContext.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private EnemyPositionsProvider _positionsProvider;
        [SerializeField] private PlayerFacade _player;

        [SerializeField] private float _minSpawnCooldown;
        [SerializeField] private float _maxSpawnCooldown;

        private readonly UnityTimer _timer = new();
        
        public event Action OnEnemyDead;
        
        public int DestroyedEnemies { get; private set; }

        private void Start()
        {
            ResetSpawnCooldown();
        }

        private void FixedUpdate()
        {
            if (!_timer.IsExpired())
                return;

            Spawn();
            ResetSpawnCooldown();
        }

        private void Spawn()
        {
            var spawnPosition = _positionsProvider.NextSpawnPosition();
            var destination = _positionsProvider.NextDestination();
            
            var enemy = _enemyPool.Spawn(spawnPosition, Quaternion.identity);

            enemy.SetDestination(destination);
            enemy.SetTarget(_player);
            
            enemy.OnDead += OnEnemyDeadHandler;
        }

        private void OnEnemyDeadHandler(EnemyFacade enemy) 
        {
            enemy.OnDead -= OnEnemyDeadHandler;
            
            enemy.SetTarget(null);
            
            DestroyedEnemies++;
            _enemyPool.Despawn(enemy);

            OnEnemyDead?.Invoke();
        }

        private void ResetSpawnCooldown()
        {
            _timer.SetRandom(_minSpawnCooldown, _maxSpawnCooldown);
        }
    }
}
