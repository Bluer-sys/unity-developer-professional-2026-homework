using System;
using Game.GameObjects.Ship;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GameContext.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;

        [SerializeField] private float _minSpawnCooldown;
        [SerializeField] private float _maxSpawnCooldown;
        
        [SerializeField] private PlayerFacade _player;
        
        private float _spawnCooldown;
        private float _lastSpawnTime;
        private int _spawnIndex;
        private int _attackIndex;

        public event Action OnEnemyDead;
        
        public int DestroyedEnemies { get; private set; }

        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
        }
        
        private void Start()
        {
            ResetSpawnCooldown();
        }
        
        private void FixedUpdate()
        {
            float time = Time.fixedTime;

            if (time - _lastSpawnTime < _spawnCooldown)
                return;

            Spawn();
            ResetSpawnCooldown();
        }

        private void Spawn()
        {
            var spawnPosition = NextSpawnPosition();
            var destination = NextDestination();
            
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
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _lastSpawnTime = Time.fixedTime;
        }

        private Vector3 NextSpawnPosition()
        {
            if (_spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                _spawnIndex = 0;
            }

            return _spawnPositions[_spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (_attackIndex >= _attackPositions.Length)
            {
                _attackPositions.Shuffle();
                _attackIndex = 0;
            }

            return _attackPositions[_attackIndex++].position;
        }
    }
}
