using UnityEngine;

namespace Game.Weapon
{
    public class WeaponEffectsController : MonoBehaviour
    {
        [SerializeField] private CooldownWeapon _commonWeapon;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _fireVFX;
        [SerializeField] private AudioClip _fireSFX;
        
        
        private void OnEnable()
        {
            _commonWeapon.OnFired += OnFired;
        }

        private void OnDisable()
        {
            _commonWeapon.OnFired -= OnFired;
        }

        private void OnFired()
        {
            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);

            if (_fireVFX)
                _fireVFX.Play();
        }
    }
}
