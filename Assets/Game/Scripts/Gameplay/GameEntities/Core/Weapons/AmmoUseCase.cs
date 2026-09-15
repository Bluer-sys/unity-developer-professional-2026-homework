namespace Game.Gameplay
{
    public static class AmmoUseCase
    {
        public static bool HasAmmoWeapon(IGameEntity entity)
        {
            return entity.TryGetWeapon(out IGameEntity weapon) && weapon.TryGetAmmo(out _);
        }

        public static void AddAmmo(IGameEntity entity, int amount)
        {
            entity.GetWeapon().GetAmmo().Value += amount;
        }
    }
}
