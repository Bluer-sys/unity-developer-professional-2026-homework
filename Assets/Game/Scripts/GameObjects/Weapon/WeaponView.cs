using UnityEngine;

namespace Game.GameObjects.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private CooldownWeapon _weapon;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _fireVFX;
        [SerializeField] private AudioClip _fireSFX;
        
        
        private void OnEnable()
        {
            _weapon.OnFired += OnFired;
        }

        private void OnDisable()
        {
            _weapon.OnFired -= OnFired;
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
