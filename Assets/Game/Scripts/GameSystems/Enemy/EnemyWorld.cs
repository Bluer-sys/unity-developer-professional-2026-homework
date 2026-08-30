using System;
using System.Collections.Generic;
using Fusion;
using Game.Core;
using Game.GameObjects;
using UnityEngine;

namespace Game
{
    public class EnemyWorld : NetworkBehaviour
    {
        public event Action OnEnemyDead;
        
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _moveTarget;
        
        private readonly List<NetworkObject> _spawnedEnemies = new();
        
        public void Spawn(Vector3 at)
        {
            NetworkObject obj = Runner.Spawn(_enemyPrefab, at, Quaternion.identity);
            obj.GetComponent<NetworkTransform>().Teleport(at);
            Physics.SyncTransforms();

            var enemy = obj.GetBehaviour<Enemy>();
            var moveDir = (_moveTarget.position - enemy.transform.position).normalized;

            enemy.SetMoveDirection(moveDir);

            _spawnedEnemies.Add(obj);
        }

        public void DespawnOrphans()
        {
            var orphans = UnityEngine.Pool.ListPool<NetworkObject>.Get();

            foreach (NetworkObject obj in _spawnedEnemies)
            {
                if (obj.TryGetBehaviour(out Enemy enemy) && 
                    obj.TryGetBehaviour(out HealthComponent health) && 
                    (enemy.IsPortalReached || health.IsDead))
                {
                    if(health.IsDead)
                        OnEnemyDead?.Invoke();
                    
                    Runner.Despawn(obj);
                    orphans.Add(obj);
                }
            }

            foreach (NetworkObject obj in orphans)
                _spawnedEnemies.Remove(obj);

            UnityEngine.Pool.ListPool<NetworkObject>.Release(orphans);
        }
    }
}
