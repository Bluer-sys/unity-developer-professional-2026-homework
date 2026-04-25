using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class LavaView : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        private TriggerComponent _triggerComponent;

        [Inject]
        private void Construct(TriggerComponent triggerComponent)
        {
            _triggerComponent = triggerComponent;
        }

        private void OnEnable() => _triggerComponent.OnEntered += OnEntered;

        private void OnDisable() => _triggerComponent.OnEntered -= OnEntered;

        private void OnEntered(Collider2D _) => _audioSource.Play();
    }
}
