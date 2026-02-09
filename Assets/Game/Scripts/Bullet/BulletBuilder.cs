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
        
        private Action<BulletBuilder> _onDead;


        public BulletBuilder SetDirection(Vector2 direction)
        {
            _movement.SetDirection(direction);
            
            return this;
        }

        public BulletBuilder SetSpeed(float speed)
        {
            _movement.SetSpeed(speed);
            
            return this;
        }

        public BulletBuilder SetLayer(int layer)
        {
            gameObject.layer = layer;
            
            return this;
        }

        public BulletBuilder SetVfx(bool isRedVfx)
        {
            _blueVFX.SetActive(!isRedVfx);
            _redVFX.SetActive(isRedVfx);
            
            return this;
        }

        public BulletBuilder SetDamage(int damage)
        {
            _collisionDamageReceiver.SetDamage(damage);
            
            return this;
        }

        public BulletBuilder OnDead(Action<BulletBuilder> onDead)
        {
            _onDead = onDead;

            _bulletFacade.OnDead -= OnDeadHandler;
            _bulletFacade.OnDead += OnDeadHandler;
            
            return this;
        }

        public BulletFacade GetFacade()
        {
            return _bulletFacade;
        }

        public void SetToDefault()
        {
            SetDirection(Vector2.up);
            SetLayer(0);
            SetVfx(false);
            SetDamage(1);
            OnDead(null);
        }

        private void OnDeadHandler(BulletFacade bullet)
        {
            _onDead?.Invoke(this);
            _onDead = null;
            
            _bulletFacade.OnDead -= OnDeadHandler;
        }
    }
}
