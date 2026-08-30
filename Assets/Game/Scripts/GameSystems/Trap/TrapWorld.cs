using System.Collections.Generic;
using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public class TrapWorld : SimulationBehaviour
    {
        [SerializeField] private TrapCatalog _catalog;
        
        private readonly List<NetworkObject> _spawnedTraps = new();

        public override void FixedUpdateNetwork()
        {
            DespawnOrphans();
        }

        public void Spawn(TrapType type, Vector3 at)
        {
            var config = _catalog.Configs[type];
            var trap = Runner.Spawn(config.Prefab, at, Quaternion.identity);
            var lifetime = trap.GetBehaviour<LifetimeComponent>();

            lifetime?.SetAction(() => Despawn(trap));
            lifetime?.ResetTimer(config.Lifetime);

            _spawnedTraps.Add(trap);
        }

        private void DespawnOrphans()
        {
            var orphans = UnityEngine.Pool.ListPool<NetworkObject>.Get();

            foreach (NetworkObject obj in _spawnedTraps)
            {
                if (obj.TryGetBehaviour(out HealthComponent health) && 
                    health.IsDead)
                {
                    Runner.Despawn(obj);
                    orphans.Add(obj);
                }
            }

            foreach (NetworkObject obj in orphans)
                _spawnedTraps.Remove(obj);

            UnityEngine.Pool.ListPool<NetworkObject>.Release(orphans);
        }
        
        private void Despawn(NetworkObject obj) 
        {
            Runner.Despawn(obj);
            _spawnedTraps.Remove(obj);
        }
    }
}
