using Game.Common;
using Game.Data;
using UnityEngine;

namespace Game.Visual
{
    public class DeathEffectsController : MonoBehaviour
    {
        [SerializeField] private Transform _viewTransform;
        
        private HealthComponent _health;
        private ShipViewConfig _viewConfig;

        public void Construct(HealthComponent health, ShipViewConfig viewConfig)
        {
            _health = health;
            _viewConfig = viewConfig;

            _health.OnDead += OnDead;
        }
        
        private void OnDestroy()
        {
            _health.OnDead -= OnDead;
        }

        private void OnDead()
        {
            ParticleSystem prefab = _viewConfig.DestroyEffectPrefab;
            Instantiate(prefab, _viewTransform.position, prefab.transform.rotation);
        }
    }
}
