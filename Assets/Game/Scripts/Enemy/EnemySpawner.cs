using Game.Player;
using Modules.UI;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private ScoreView _scoreView;
        
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;

        [SerializeField] private float _minSpawnCooldown;
        [SerializeField] private float _maxSpawnCooldown;
        
        [SerializeField] private PlayerFacade _player;
        
        private float _spawnCooldown;
        private int _destroyedEnemies;
        private float _lastSpawnTime;
        private int _spawnIndex;
        private int _attackIndex;
        
        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
            _scoreView.SetValue(_destroyedEnemies);
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
            
            EnemyFacade enemy = _enemyPool.Spawn(spawnPosition);

            enemy.SetDestination(destination);
            enemy.SetTarget(_player);
            
            enemy.OnDead += OnEnemyDead;
        }

        private void OnEnemyDead(EnemyFacade enemy) 
        {
            enemy.OnDead -= OnEnemyDead;
            
            _destroyedEnemies++;
            _scoreView.SetValue(_destroyedEnemies);
            _enemyPool.Despawn(enemy);
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
