using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Lava : IInitializable, IDisposable
    {
        private readonly TriggerComponent _triggerComponent;

        public Lava(TriggerComponent triggerComponent)
        {
            _triggerComponent = triggerComponent;
        }

        void IInitializable.Initialize() =>
            _triggerComponent.OnEntered += OnEntered;

        void IDisposable.Dispose() =>
            _triggerComponent.OnEntered -= OnEntered;

        private void OnEntered(Collider2D col)
        {
            GameEntity entity = col.GetComponentInParent<GameEntity>();
            if (entity == null)
                return;

            if (!entity.TryGet(out HealthComponent health))
                return;

            health.SetZero();
        }
    }
}
