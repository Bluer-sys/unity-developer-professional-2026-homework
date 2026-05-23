using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class DeathViewComponent : IInitializable, IDisposable
    {
        private static readonly int _death = Animator.StringToHash("Death");
        
        private readonly HealthComponent _healthComponent;
        private readonly Animator _animator;

        public DeathViewComponent(HealthComponent healthComponent, Animator animator)
        {
            _healthComponent = healthComponent;
            _animator = animator;
        }

        public void Initialize() => _healthComponent.OnDied += OnDied;
        public void Dispose() => _healthComponent.OnDied -= OnDied;

        private void OnDied() => _animator.SetTrigger(_death);
    }
}
