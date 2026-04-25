using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class CharacterView : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _tossClip;
        [SerializeField] private AudioClip _pushClip;
        [SerializeField] private AudioClip _takeDamageClip;

        private JumpComponent _jumpComponent;
        private Character _character;
        private HealthComponent _healthComponent;

        private float _previousHealth = float.MaxValue;

        [Inject]
        private void Construct(
            JumpComponent jumpComponent,
            Character character,
            HealthComponent healthComponent)
        {
            _jumpComponent = jumpComponent;
            _character = character;
            _healthComponent = healthComponent;
        }

        private void OnEnable()
        {
            _previousHealth = _healthComponent.CurrentHealth;

            _jumpComponent.OnJumped += OnJumped;
            _character.OnTossed += OnTossed;
            _character.OnPushedUp += OnPushedUp;
            _healthComponent.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _jumpComponent.OnJumped -= OnJumped;
            _character.OnTossed -= OnTossed;
            _character.OnPushedUp -= OnPushedUp;
            _healthComponent.OnHealthChanged -= OnHealthChanged;
        }

        private void OnJumped() => _audioSource.PlayOneShot(_jumpClip);
        private void OnTossed() => _audioSource.PlayOneShot(_tossClip);
        private void OnPushedUp() => _audioSource.PlayOneShot(_pushClip);

        private void OnHealthChanged(float current)
        {
            if (current < _previousHealth)
                _audioSource.PlayOneShot(_takeDamageClip);

            _previousHealth = current;
        }
    }
}
