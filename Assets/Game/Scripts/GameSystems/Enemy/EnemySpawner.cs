using Fusion;
using Game.Core;
using Game.GameObjects;
using UnityEngine;

namespace Game
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private SpawnPointService _spawnPointService;
        [SerializeField] private Portal _portal;
        [SerializeField] private float _spawnInterval;

        [Networked] private TickTimer SpawnDelayTimestamp { get; set; }

        public override void Spawned()
        {
            ResetTimer();
            
            _portal.OnEnemyReached += Despawn;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _portal.OnEnemyReached -= Despawn;
        }

        public override void FixedUpdateNetwork()
        {
            if (!SpawnDelayTimestamp.Expired(Runner))
                return;

            SpawnEnemy();
            ResetTimer();
        }

        private void SpawnEnemy()
        {
            NetworkObject networkObj = Runner.Spawn(_enemyPrefab, _spawnPointService.GetRandomSpawnPosition(), Quaternion.identity);
            var enemy = networkObj.GetBehaviour<Enemy>();
            var health = networkObj.GetBehaviour<HealthComponent>();
            var moveDir = (_portal.Center.position - enemy.transform.position).normalized;
            
            enemy.SetMoveDirection(moveDir);
            
            health.OnHealthOver += Despawn;
        }

        public void Despawn(Enemy enemy)
        {
            enemy.TryGetComponent(out HealthComponent health);
            Despawn(health);
        }
        
        private void Despawn(HealthComponent health)
        {
            health.OnHealthOver -= Despawn;
            Runner.Despawn(health.Object);
        }

        private void ResetTimer()
        {
            SpawnDelayTimestamp = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
        }
    }
}
