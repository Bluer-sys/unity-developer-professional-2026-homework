using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class WeaponViewInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private ParticleSystem _fireEffect;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);

        public override void Install(IGameEntity entity)
        {
            entity.AddBehaviour(new WeaponFeedbackBehaviour(_fireEffect, _audioSource, _pitchRange));
        }
    }
}
