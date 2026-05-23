using UnityEngine;
using Zenject;

namespace Game
{
    public class MoveViewComponent : ITickable
    {
        private static readonly int _isMoving = Animator.StringToHash("IsMoving");

        private readonly Animator _animator;
        private readonly MoveTransformComponent _moveComponent;

        public MoveViewComponent(MoveTransformComponent moveComponent, Animator animator)
        {
            _moveComponent = moveComponent;
            _animator = animator;
        }

        public void Tick()
        {
            _animator.SetBool(_isMoving, _moveComponent.MoveDirection != Vector2.zero);
        }
    }
}
