using System;
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
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Transform _moveTarget;

        [Networked] private TickTimer SpawnDelayTimestamp { get; set; }

        public override void Spawned()
        {
            ResetTimer();
        }

        public override void FixedUpdateNetwork()
        {
            if (!SpawnDelayTimestamp.Expired(Runner))
                return;

            Spawn();
            ResetTimer();
        }

        private void Spawn()
        {
            Vector3 spawnPos = _spawnPointService.GetRandomSpawnPosition();
            
            NetworkObject networkObj = Runner.Spawn(_enemyPrefab, spawnPos, Quaternion.identity);
            networkObj.GetComponent<NetworkTransform>().Teleport(spawnPos);
            Physics.SyncTransforms();
            
            var enemy = networkObj.GetBehaviour<Enemy>();
            var health = networkObj.GetBehaviour<HealthComponent>();
            var moveDir = (_moveTarget.position - enemy.transform.position).normalized;
            
            enemy.SetMoveDirection(moveDir);
            
            health.OnDeath += Despawn;
            enemy.OnPortalReached += Despawn;
        }

        private void Despawn(NetworkObject obj)
        {
            obj.GetBehaviour<HealthComponent>().OnDeath -= Despawn;
            obj.GetBehaviour<Enemy>().OnPortalReached -= Despawn;
            
            Runner.Despawn(obj);
        }

        private void ResetTimer()
        {
            SpawnDelayTimestamp = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
        }
    }
}
