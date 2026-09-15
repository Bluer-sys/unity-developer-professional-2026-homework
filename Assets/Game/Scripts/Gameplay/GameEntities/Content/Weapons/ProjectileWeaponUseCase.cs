using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class ProjectileWeaponUseCase
    {
        public static bool CanFire(IGameEntity weapon)
        {
            return weapon.GetAmmo().Value > 0 && weapon.GetFireCooldown().IsCompleted();
        }

        public static bool TryFire(IGameEntity weapon, IGameEntity owner, IEntityPool<IGameEntity> pool)
        {
            if (!owner.IsAlive() || !CanFire(weapon))
                return false;

            Transform firePoint = weapon.GetFirePoint();
            float spread = weapon.GetSpreadAngle().Value;
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(spreadX, spreadY, 0);

            weapon.GetAmmo().Value--;
            weapon.GetFireCooldown().ResetTime();
            BulletUseCase.Spawn(pool, owner, firePoint.position, rotation);
            weapon.GetFireEvent().Invoke();
            
            return true;
        }
    }
}
