using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class MovementInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private RootMotionSource _rootMotion;
        [SerializeField] private float _rootMotionMultiplier = 1;

        public override void Install(IGameEntity entity)
        {
            entity.AddRigidbody(_rigidbody);
            entity.AddRootMotion(_rootMotion);
            entity.AddMovementDirection(new Variable<Vector3>());
            entity.AddRootMotionMultiplier(new Const<float>(_rootMotionMultiplier));
            entity.AddBehaviour(new RootMotionMovementBehaviour());
        }
    }
}
