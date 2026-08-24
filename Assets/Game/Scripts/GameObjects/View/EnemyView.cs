using Fusion;
using Game.Core;
using Game.GameObjects;
using UnityEngine;

namespace Game.View
{
    public class EnemyView : NetworkBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private HealthComponent _enemyHealth;
        
        [SerializeField] private GameObject _deathVfx;
        [SerializeField] private Vector3 _deathVfxOffset;
        
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (hasState)
                PlayDeath();
        }
       
        private void PlayDeath()
        {
            Instantiate(_deathVfx, transform.position + _deathVfxOffset, Quaternion.identity);
        }
    }
}
