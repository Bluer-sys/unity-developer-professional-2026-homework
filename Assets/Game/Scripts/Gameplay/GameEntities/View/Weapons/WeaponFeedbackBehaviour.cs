using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class WeaponFeedbackBehaviour : 
        IEntityInit<IGameEntity>, 
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private readonly ParticleSystem _fireEffect;
        private readonly AudioSource _audioSource;
        private readonly Vector2 _pitchRange;
        private ISignal _fireEvent;

        public WeaponFeedbackBehaviour(ParticleSystem fireEffect, AudioSource audioSource, Vector2 pitchRange)
        {
            _fireEffect = fireEffect;
            _audioSource = audioSource;
            _pitchRange = pitchRange;
        }

        public void Init(IGameEntity entity)
        {
            _fireEvent = entity.GetFireEvent();
        }

        public void Enable(IGameEntity entity)
        {
            _fireEvent.OnEvent += OnFire;
        }

        public void Disable(IGameEntity entity)
        {
            _fireEvent.OnEvent -= OnFire;
        }

        private void OnFire()
        {
            _fireEffect.Play(true);
            _audioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
            _audioSource.Play();
        }
    }
}
