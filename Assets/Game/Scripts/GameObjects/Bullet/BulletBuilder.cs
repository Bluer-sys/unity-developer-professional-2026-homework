using System;
using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletBuilder : MonoBehaviour
    {
        private Bullet _bullet;
        private BulletView _bulletView;
        private MovementComponent _movement;
        
        private Vector2 _direction;
        private BulletConfig _config;
        private Action<BulletBuilder> _onDead;

        public void Construct(Bullet bullet,
                              BulletView bulletView, 
                              MovementComponent movement)
        {
            _bullet = bullet;
            _bulletView = bulletView;
            _movement = movement;
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
            if(_config == null)
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
