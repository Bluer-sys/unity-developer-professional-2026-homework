using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class BulletInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _speed = 45;
        [SerializeField] private float _lifetime = 3;
        [SerializeField] private LayerMask _collisionMask = Physics.DefaultRaycastLayers;

        public override void Install(IGameEntity entity)
        {
            entity.AddRigidbody(_rigidbody);
            entity.AddOwner(new Variable<IGameEntity>());
            entity.AddDamage(new Const<int>(_damage));
            entity.AddBulletSpeed(new Const<float>(_speed));
            entity.AddBulletRadius(new Const<float>(_collider.radius));
            entity.AddLifetime(new Cooldown(_lifetime));
            entity.AddIsFlying(new Variable<bool>());
            entity.AddBehaviour(new BulletFlightBehaviour(_collisionMask));
        }
    }
}
