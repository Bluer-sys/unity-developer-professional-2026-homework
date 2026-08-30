using Fusion;
using UnityEngine;

namespace Game
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private SpawnPointService _spawnPointService;
        [SerializeField] private EnemyWorld _enemyWorld;
        [SerializeField] private float _spawnInterval;

        [Networked] private TickTimer SpawnDelayTimestamp { get; set; }

        public override void Spawned()
        {
            ResetTimer();
        }

        public override void FixedUpdateNetwork()
        {
            _enemyWorld.DespawnOrphans();
            
            if (!SpawnDelayTimestamp.Expired(Runner))
                return;

            Vector3 spawnPos = _spawnPointService.GetRandomSpawnPosition();
            
            _enemyWorld.Spawn(spawnPos);
            
            ResetTimer();
        }

        private void ResetTimer()
        {
            SpawnDelayTimestamp = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
        }
    }
}
