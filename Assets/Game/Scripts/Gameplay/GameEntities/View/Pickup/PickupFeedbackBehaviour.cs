using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PickupFeedbackBehaviour :
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private readonly GameObject _visual;
        private readonly ParticleSystem _effect;
        private readonly AudioSource _audioSource;
        private IValue<bool> _isCollected;
        private ISignal _pickupEvent;

        public PickupFeedbackBehaviour(GameObject visual, ParticleSystem effect, AudioSource audioSource)
        {
            _visual = visual;
            _effect = effect;
            _audioSource = audioSource;
        }

        public void Init(IGameEntity entity)
        {
            _isCollected = entity.GetIsCollected();
            _pickupEvent = entity.GetPickupEvent();
        }

        public void Enable(IGameEntity entity)
        {
            _pickupEvent.OnEvent += OnPickup;
            _visual.SetActive(!_isCollected.Value);
        }

        public void Disable(IGameEntity entity)
        {
            _pickupEvent.OnEvent -= OnPickup;
            _effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _audioSource.Stop();
        }

        private void OnPickup()
        {
            _visual.SetActive(false);
            _effect.Play(true);
            _audioSource.Play();
        }
    }
}
