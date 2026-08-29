using System;
using System.Collections.Generic;
using Fusion;
using Game.Core;
using Game.GameObjects;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Game
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private SpawnPointService _spawnPointService;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Transform _moveTarget;

        private List<NetworkObject> _spawnedEnemies = new();

        [Networked] private TickTimer SpawnDelayTimestamp { get; set; }

        public override void Spawned()
        {
            ResetTimer();
        }

        public override void FixedUpdateNetwork()
        {
            TryDespawn();
            
            if (!SpawnDelayTimestamp.Expired(Runner))
                return;
            
            Spawn();
            ResetTimer();
        }

        private void Spawn()
        {
            Vector3 spawnPos = _spawnPointService.GetRandomSpawnPosition();
            
            NetworkObject obj = Runner.Spawn(_enemyPrefab, spawnPos, Quaternion.identity);
            obj.GetComponent<NetworkTransform>().Teleport(spawnPos);
            Physics.SyncTransforms();
            
            var enemy = obj.GetBehaviour<Enemy>();
            var moveDir = (_moveTarget.position - enemy.transform.position).normalized;
            
            enemy.SetMoveDirection(moveDir);
            
            _spawnedEnemies.Add(obj);
        }

        private void TryDespawn()
        {
            var orphans = UnityEngine.Pool.ListPool<NetworkObject>.Get();
            
            foreach (NetworkObject obj in _spawnedEnemies)
            {
                if (obj.TryGetBehaviour(out Enemy enemy) && 
                    obj.TryGetBehaviour(out HealthComponent health) &&
                    (enemy.IsPortalReached || health.IsDead))
                {
                    Runner.Despawn(obj);
                    orphans.Add(obj);
                }
            }

            foreach (NetworkObject obj in orphans)
                _spawnedEnemies.Remove(obj);
            
            UnityEngine.Pool.ListPool<NetworkObject>.Release(orphans);
        }

        private void ResetTimer()
        {
            SpawnDelayTimestamp = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
        }
    }
}
