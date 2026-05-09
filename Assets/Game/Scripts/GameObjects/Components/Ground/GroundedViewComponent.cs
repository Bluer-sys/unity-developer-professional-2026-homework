using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GroundedViewComponent : IInitializable, IDisposable
    {
        private static readonly int _isGrounded = Animator.StringToHash("IsGrounded");
        
        private readonly GroundedComponent _groundedComponent;
        private readonly Animator _animator;

        public GroundedViewComponent(GroundedComponent groundedComponent, Animator animator)
        {
            _groundedComponent = groundedComponent;
            _animator = animator;
        }

        public void Initialize()
        {
            _groundedComponent.OnGrounded += OnGrounded;
        }

        public void Dispose()
        {
            _groundedComponent.OnGrounded -= OnGrounded;
        }

        private void OnGrounded(bool isGrounded)
        {
            _animator.SetBool(_isGrounded, isGrounded);
        }
    }
}
