using DG.Tweening;
using UnityEngine;

namespace Game.GameObjects
{
    public class TakeDamageViewComponent : MonoBehaviour
    {
        [SerializeField] private ShipViewConfig _viewConfig;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ShipMaterial _shipMaterial;

        private Tweener _damageAnimation;


        private void OnEnable()
        {
            _health.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        { 
            AnimateDamage();
        }
        
        private void AnimateDamage()
        {
            _damageAnimation?.Kill();
            _damageAnimation = DOVirtual
                               .Float(0f, 1f, _viewConfig.HitDuration, SetHitProperty)
                               .SetLink(_shipMaterial.Renderer.gameObject)
                               .OnComplete(() => _damageAnimation = null);

            if (_viewConfig.DamageSfx)
                _audioSource.PlayOneShot(_viewConfig.DamageSfx);
        }

        private void SetHitProperty(float progress)
        {
            _shipMaterial.Material.SetFloat(_viewConfig.HitPropertyName, _viewConfig.HitAnimationCurve.Evaluate(progress));
        }
    }
}
