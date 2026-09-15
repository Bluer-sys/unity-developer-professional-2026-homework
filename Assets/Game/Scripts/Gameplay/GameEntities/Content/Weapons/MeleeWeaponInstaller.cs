using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class MeleeWeaponInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private Transform _hitPoint;
        [SerializeField] private float _hitRadius = 0.2f;
        [SerializeField] private float _attackDistance = 0.8f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private LayerMask _hitMask;

        public override void Install(IGameEntity entity)
        {
            entity.AddHitPoint(_hitPoint);
            entity.AddHitRadius(new Const<float>(_hitRadius));
            entity.AddAttackDistance(new Const<float>(_attackDistance));
            entity.AddHitMask(new Const<LayerMask>(_hitMask));
            entity.AddDamage(new Const<int>(_damage));
            entity.AddFireCooldown(new Cooldown(_cooldown, 0));
            entity.AddBehaviour(new WeaponCooldownBehaviour());
        }
    }
}
