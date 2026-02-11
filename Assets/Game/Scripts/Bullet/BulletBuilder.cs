using System;
using Game.Collision;
using Game.Common;
using UnityEngine;

namespace Game.Bullet
{
    public class BulletBuilder : MonoBehaviour
    {
        [SerializeField] private BulletFacade _bulletFacade;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private CollisionDamageReceiver _collisionDamageReceiver;

        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;
        
        private Vector2 _direction;
        private float _speed;
        private int _layer;
        private bool _isRedVfx;
        private int _damage;
        private Action<BulletBuilder> _onDead;

        public BulletBuilder WithDirection(Vector2 direction)
        {
            _direction = direction;
            return this;
        }

        public BulletBuilder WithSpeed(float speed)
        {
            _speed = speed;
            return this;
        }

        public BulletBuilder WithLayer(int layer)
        {
            _layer = layer;
            return this;
        }

        public BulletBuilder WithVfx(bool isRedVfx)
        {
            _isRedVfx = isRedVfx;
            return this;
        }

        public BulletBuilder WithDamage(int damage)
        {
            _damage = damage;
            return this;
        }

        public BulletBuilder OnDead(Action<BulletBuilder> onDead)
        {
            _onDead = onDead;
            return this;
        }

        public BulletFacade Build()
        {
            _movement.SetDirection(_direction);
            _movement.SetSpeed(_speed);
            _collisionDamageReceiver.SetDamage(_damage);

            gameObject.layer = _layer;

            _blueVFX.SetActive(!_isRedVfx);
            _redVFX.SetActive(_isRedVfx);
            
            _bulletFacade.OnDead -= OnDeadHandler;
            _bulletFacade.OnDead += OnDeadHandler;
            
            return _bulletFacade;
        }

        public void SetToDefault()
        {
            WithDirection(default);
            WithSpeed(default);
            WithDamage(default);
            WithLayer(default);
            WithVfx(default);
            OnDead(default);
        }

        private void OnDeadHandler(BulletFacade bullet)
        {
            _onDead?.Invoke(this);
            _onDead = null;
            
            _bulletFacade.OnDead -= OnDeadHandler;
        }
    }
}
