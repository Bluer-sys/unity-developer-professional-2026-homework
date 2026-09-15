using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class MovementAnimationBehaviour : 
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>, 
        IEntityFixedTick<IGameEntity>
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private IValue<int> _health;
        private IValue<Vector3> _movementDirection;
        private Animator _animator;

        public void Init(IGameEntity entity)
        {
            _health = entity.GetHealth();
            _movementDirection = entity.GetMovementDirection();
            _animator = entity.GetAnimator();
        }

        public void Enable(IGameEntity entity)
        {
            UpdateAnimation();
        }

        public void Disable(IGameEntity entity)
        {
            _animator.SetBool(IsMoving, false);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            _animator.SetBool(IsMoving, _health.Value > 0 && _movementDirection.Value != Vector3.zero);
        }
    }
}
