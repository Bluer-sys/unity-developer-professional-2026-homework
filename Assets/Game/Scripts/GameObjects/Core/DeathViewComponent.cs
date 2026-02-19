using UnityEngine;

namespace Game.GameObjects
{
    public class DeathViewComponent : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private HealthComponent _health;

        private void OnEnable()
        {
            _health.OnDead += OnDead;
        }

        private void OnDisable()
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
