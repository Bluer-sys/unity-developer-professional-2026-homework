using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class CharacterView : MonoBehaviour
    {
        private static readonly int _blowForward = Animator.StringToHash("BlowForward");
        private static readonly int _blowUp = Animator.StringToHash("BlowUp");
        
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _tossClip;
        [SerializeField] private ParticleSystem _tossFx;
        [SerializeField] private AudioClip _pushClip;
        [SerializeField] private ParticleSystem _pushFx;
        [SerializeField] private AudioClip _takeDamageClip;

        private ForceAbilityComponent _jumpComponent;
        private Character _character;
        private HealthComponent _healthComponent;
        private LookComponent _lookComponent;
        private Animator _animator;
        private AudioSource _audioSource;

        [Inject]
        private void Construct(
            [Inject(Id = "Jump")] ForceAbilityComponent jumpComponent,
            Character character,
            HealthComponent healthComponent,
            GroundedComponent groundedComponent,
            LookComponent lookComponent,
            Animator animator,
            AudioSource audioSource)
        {
            _lookComponent = lookComponent;
            _jumpComponent = jumpComponent;
            _character = character;
            _healthComponent = healthComponent;
            _animator = animator;
            _audioSource = audioSource;
        }

        private void OnEnable()
        {
            _jumpComponent.OnPerformed += OnJumped;
            _character.OnPushed += OnPushed;
            _character.OnBlownUp += OnBlownUp;
            _healthComponent.OnHealthDecreased += OnHealthDecreased;
        }

        private void OnDisable()
        {
            _jumpComponent.OnPerformed -= OnJumped;
            _character.OnPushed -= OnPushed;
            _character.OnBlownUp -= OnBlownUp;
            _healthComponent.OnHealthDecreased -= OnHealthDecreased;
        }

        private void OnJumped()
        {
            _audioSource.PlayOneShot(_jumpClip);
        }

        private void OnPushed()
        {
            _audioSource.PlayOneShot(_tossClip);
            _animator.SetTrigger(_blowForward);

            _pushFx.transform.localScale = _lookComponent.CurrentDirection > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            _pushFx.Play();
        }

        private void OnBlownUp()
        {
            _audioSource.PlayOneShot(_pushClip);
            _animator.SetTrigger(_blowUp);

            _tossFx.transform.localScale = _lookComponent.CurrentDirection > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            _tossFx.Play();
        }

        private void OnHealthDecreased()
        {
            _audioSource.PlayOneShot(_takeDamageClip);
        }
    }
}
