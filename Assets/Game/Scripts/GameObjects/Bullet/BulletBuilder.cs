using System;
using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletBuilder : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CollisionHandler _collisionHandler;
        
        [SerializeField] private Bullet _bullet;
        [SerializeField] private BulletView _bulletView;
        [SerializeField] private MovementComponent _movement;

        private Vector2 _direction;
        private BulletConfig _config;
        private Action<BulletBuilder> _onDead;
        
        public BulletBuilder Construct(TransformBounds bounds)
        {
            _bullet.Construct(bounds, _collisionHandler);
            _bulletView.Construct(_bullet);
            _movement.Construct(_rigidbody, 0);

            return this;
        }

        public BulletBuilder WithConfig(BulletConfig config)
        {
            _config = config;
            return this;
        }

        public BulletBuilder WithDirection(Vector2 direction)
        {
            _direction = direction;
            return this;
        }

        public BulletBuilder OnDead(Action<BulletBuilder> onDead)
        {
            _onDead = onDead;
            return this;
        }

        public Bullet Build()
        {
            if (_config == null)
                throw new InvalidOperationException("Config is not set!");

            _movement.SetDirection(_direction);
            _movement.SetSpeed(_config.Speed);

            gameObject.layer = _config.GetLayer();

            _bulletView.SetVfx(_config.IsRedVfx);
            _bulletView.SetExplosionPrefab(_config.ExplosionPrefab);

            _bullet.SetDamage(_config.Damage);

            _bullet.OnDead -= OnDeadHandler;
            _bullet.OnDead += OnDeadHandler;

            return _bullet;
        }

        public void SetToDefault()
        {
            WithDirection(default);
            WithConfig(default);
            OnDead(default);
        }

        private void OnDeadHandler()
        {
            _onDead?.Invoke(this);
            _onDead = null;

            _bullet.OnDead -= OnDeadHandler;
        }
    }
}
