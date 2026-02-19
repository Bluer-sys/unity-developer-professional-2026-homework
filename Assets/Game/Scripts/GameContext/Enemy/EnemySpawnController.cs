using Game.Utils;
using UnityEngine;

namespace Game.GameContext
{
    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _spawner;
        [SerializeField] private float _minSpawnCooldown;
        [SerializeField] private float _maxSpawnCooldown;
        
        private readonly UnityTimer _timer = new();
        
        private void Start()
        {
            ResetSpawnCooldown();
        }

        private void FixedUpdate()
        {
            if (!_timer.IsExpired())
                return;

            _spawner.Spawn();
            ResetSpawnCooldown();
        }

        private void ResetSpawnCooldown()
        {
            _timer.SetRandom(_minSpawnCooldown, _maxSpawnCooldown);
        }
    }
}
