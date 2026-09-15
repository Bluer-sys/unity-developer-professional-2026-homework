using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class ProjectileWeaponInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private int _ammo = 8;
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private float _spreadAngle = 0.25f;

        public override void Install(IGameEntity entity)
        {
            entity.AddFirePoint(_firePoint);
            entity.AddAmmo(new ReactiveVariable<int>(_ammo));
            entity.AddFireCooldown(new Cooldown(_cooldown, 0));
            entity.AddSpreadAngle(new Const<float>(_spreadAngle));
            entity.AddFireEvent(new Atomic.Elements.Event());
            entity.AddBehaviour(new WeaponCooldownBehaviour());
        }
    }
}
