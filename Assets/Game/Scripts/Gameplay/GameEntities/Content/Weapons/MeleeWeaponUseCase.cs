namespace Game.Gameplay
{
    public static class MeleeWeaponUseCase
    {
        public static bool CanAttack(IGameEntity weapon)
        {
            return weapon.GetFireCooldown().IsCompleted();
        }

        public static bool TryHit(IGameEntity weapon, IGameEntity owner)
        {
            if (!owner.IsAlive() || !TargetUseCase.HasTarget(owner) || !CanAttack(weapon))
                return false;

            weapon.GetFireCooldown().ResetTime();

            if (!TargetUseCase.TryFindClosest(weapon.GetHitPoint().position, weapon.GetHitRadius().Value,
                    weapon.GetHitMask().Value, owner.GetTargetPredicate(), out IGameEntity target))
                return false;

            if (!TargetUseCase.IsInRange(owner, target, weapon.GetAttackDistance().Value))
                return false;

            return target.TryTakeDamage(new DamageArgs(owner, weapon.GetDamage().Value));
        }
    }
}
