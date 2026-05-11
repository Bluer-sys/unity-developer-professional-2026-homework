using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class DamageOnContactComponent : IInitializable, IDisposable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Damage { get; private set; }
        }

        public event Action<HealthComponent> OnDamageDealt;

        private readonly Settings _settings;
        private readonly CollisionComponent _collisionComponent;

        public DamageOnContactComponent(Settings settings, CollisionComponent collisionComponent)
        {
            _settings = settings;
            _collisionComponent = collisionComponent;
        }

        void IInitializable.Initialize() =>
            _collisionComponent.OnEntered += OnEntered;

        void IDisposable.Dispose() =>
            _collisionComponent.OnEntered -= OnEntered;

        private void OnEntered(Collision2D collision)
        {
            GameEntity entity = collision.collider.GetComponentInParent<GameEntity>();
            if (entity == null)
                return;

            if (!entity.TryGet(out HealthComponent health))
                return;

            health.TakeDamage(_settings.Damage);
            OnDamageDealt?.Invoke(health);
        }
    }
}
