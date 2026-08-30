using Fusion;
using UnityEngine;

namespace Game
{
    public class EnemySpawner : NetworkBehaviour
    {
        [SerializeField] private SpawnPointService _spawnPointService;
        [SerializeField] private EnemyWorld _enemyWorld;
        [SerializeField] private GameResult _gameResult;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private int _enemyCount;
  
        private bool _launched;
        private int _spawnedCount;
        
        [Networked] private TickTimer SpawnDelayTimestamp { get; set; }

        public void Launch()
        {
            _launched = true;
        }

        public override void Spawned()
        {
            ResetTimer();
        }

        public override void FixedUpdateNetwork()
        {
            WinIfAllSpawned();
            
            if (!_launched || IsAllSpawned())
                return;
            
            if (!SpawnDelayTimestamp.Expired(Runner))
                return;

            Spawn();
            ResetTimer();
        }

        private void Spawn()
        {
            Vector3 spawnPos = _spawnPointService.GetRandomSpawnPosition();
            
            _enemyWorld.Spawn(spawnPos);
            _spawnedCount++;
        }

        private void WinIfAllSpawned()
        {
            if (_enemyWorld.IsEmpty && IsAllSpawned())
                _gameResult.Change(GameResult.Result.Win);
        }

        private bool IsAllSpawned()
        {
            return _spawnedCount >= _enemyCount;
        }

        private void ResetTimer()
        {
            SpawnDelayTimestamp = TickTimer.CreateFromSeconds(Runner, _spawnInterval);
        }
    }
}
