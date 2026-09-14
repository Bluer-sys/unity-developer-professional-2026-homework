using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterAimingAnimationBehaviour : IEntityInit<IGameEntity>, IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>, IEntityFixedTick<IGameEntity>
    {
        private static readonly int _isAimingParameter = Animator.StringToHash("IsAiming");
        private static readonly int _aimX = Animator.StringToHash("AimX");
        private static readonly int _aimZ = Animator.StringToHash("AimZ");

        private IValue<int> _health;
        private IValue<bool> _isAiming;
        private IValue<Vector3> _movementDirection;
        private Rigidbody _rigidbody;
        private Animator _animator;

        public void Init(IGameEntity entity)
        {
            _health = entity.GetHealth();
            _isAiming = entity.GetIsAiming();
            _movementDirection = entity.GetMovementDirection();
            _rigidbody = entity.GetRigidbody();
            _animator = entity.GetAnimator();
        }

        public void Enable(IGameEntity entity)
        {
            UpdateAnimation();
        }

        public void Disable(IGameEntity entity)
        {
            _animator.SetBool(_isAimingParameter, false);
            _animator.SetFloat(_aimX, 0);
            _animator.SetFloat(_aimZ, 0);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            bool isAiming = _health.Value > 0 && _isAiming.Value;
            Vector3 direction = isAiming
                ? Quaternion.Inverse(_rigidbody.rotation) * _movementDirection.Value
                : Vector3.zero;

            _animator.SetBool(_isAimingParameter, isAiming);
            _animator.SetFloat(_aimX, direction.x);
            _animator.SetFloat(_aimZ, direction.z);
        }
    }
}
