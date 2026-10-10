namespace Game.Gameplay
{
    public static class WeaponUseCase
    {
        public static bool CanAttack(IGameEntity weapon, IGameEntity owner)
        {
            return owner.IsAlive() && weapon.GetFireCooldown().IsCompleted() &&
                   weapon.GetWeaponCondition().Invoke(owner);
        }

        public static bool TryAttack(IGameEntity weapon, IGameEntity owner)
        {
            if (!CanAttack(weapon, owner))
                return false;

            weapon.GetFireCooldown().ResetTime();
            weapon.GetWeaponAction().Invoke(owner);
            return true;
        }
    }
}
