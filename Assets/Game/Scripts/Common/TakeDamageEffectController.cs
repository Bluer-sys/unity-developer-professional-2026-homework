using DG.Tweening;
using Game.Data;
using UnityEngine;

namespace Game.Common
{
    public class TakeDamageEffectController : MonoBehaviour
    {
        private HealthComponent _health;
        private ShipViewConfig _viewConfig;
        private AudioClip _damageSfx;
        private AudioSource _audioSource;
        private ShipMaterial _shipMaterial;

        private Tweener _damageAnimation;

        public void Construct(HealthComponent health,
                              ShipViewConfig viewConfig,
                              AudioClip damageSfx, 
                              AudioSource audioSource, 
                              ShipMaterial shipMaterial)
        {
            _health = health;
            _viewConfig = viewConfig;
            _damageSfx = damageSfx;
            _audioSource = audioSource;
            _shipMaterial = shipMaterial;
        }

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

            if (_damageSfx)
                _audioSource.PlayOneShot(_damageSfx);
        }

        private void SetHitProperty(float progress)
        {
            _shipMaterial.Material.SetFloat(_viewConfig.HitPropertyName, _viewConfig.HitAnimationCurve.Evaluate(progress));
        }
    }
}
