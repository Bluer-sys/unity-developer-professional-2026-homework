using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class BulletFlightBehaviour : IEntityInit<IGameEntity>, IEntityFixedTick<IGameEntity>
    {
        private readonly LayerMask _collisionMask;
        private Rigidbody _rigidbody;
        private IValue<IGameEntity> _owner;
        private IValue<float> _speed;
        private IValue<float> _radius;
        private IValue<bool> _isFlying;
        private ICooldown _lifetime;

        public BulletFlightBehaviour(LayerMask collisionMask)
        {
            _collisionMask = collisionMask;
        }

        public void Init(IGameEntity entity)
        {
            _rigidbody = entity.GetRigidbody();
            _owner = entity.GetOwner();
            _speed = entity.GetBulletSpeed();
            _radius = entity.GetBulletRadius();
            _isFlying = entity.GetIsFlying();
            _lifetime = entity.GetLifetime();
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (!_isFlying.Value)
                return;

            float stepTime = Mathf.Min(deltaTime, _lifetime.GetTime());
            float distance = _speed.Value * stepTime;
            Vector3 position = _rigidbody.position;
            Vector3 direction = _rigidbody.rotation * Vector3.forward;

            if (BulletCollisionUseCase.TryFindHit(
                    position, 
                    direction, 
                    distance, 
                    _radius.Value, 
                    _collisionMask, 
                    _owner.Value.GetTransform(),
                    out Collider collider))
            {
                BulletUseCase.Hit(entity, collider);
                return;
            }

            _rigidbody.MovePosition(position + direction * distance);
            _lifetime.Tick(deltaTime);
            
            if (_lifetime.IsCompleted())
                BulletUseCase.Despawn(entity);
        }
    }
}
