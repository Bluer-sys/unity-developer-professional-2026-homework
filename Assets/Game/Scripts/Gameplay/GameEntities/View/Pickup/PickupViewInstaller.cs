using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PickupViewInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private GameObject _visual;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private AudioSource _audioSource;

        public override void Install(IGameEntity entity)
        {
            entity.AddBehaviour(new PickupFeedbackBehaviour(_visual, _effect, _audioSource));
        }
    }
}
