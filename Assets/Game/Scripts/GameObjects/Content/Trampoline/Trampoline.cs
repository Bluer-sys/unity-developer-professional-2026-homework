using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Trampoline : IInitializable, IDisposable
    {
        private readonly TriggerComponent _triggerComponent;
        private readonly ForceTargetComponent _forceComponent;

        public Trampoline(
            TriggerComponent triggerComponent,
            ForceTargetComponent forceComponent)
        {
            _triggerComponent = triggerComponent;
            _forceComponent = forceComponent;
        }

        void IInitializable.Initialize() =>
            _triggerComponent.OnEntered += OnEntered;

        void IDisposable.Dispose() =>
            _triggerComponent.OnEntered -= OnEntered;

        private void OnEntered(Collider2D other) =>
            _forceComponent.ApplyForce(other);
    }
}
