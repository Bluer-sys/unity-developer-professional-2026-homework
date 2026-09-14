using Atomic.Elements;
using UnityEngine;

namespace Game.Gameplay
{
    public static class HealthUseCase
    {
        public static bool IsAlive(this IGameEntity entity)
        {
            return entity.GetHealth().Value > 0;
        }

        public static bool TryHeal(this IGameEntity entity, int amount)
        {
            if (amount <= 0 || !entity.IsAlive())
                return false;

            IVariable<int> health = entity.GetHealth();
            int restoredHealth = Mathf.Min(amount, entity.GetMaxHealth().Value - health.Value);

            if (restoredHealth == 0)
                return false;

            health.Value += restoredHealth;
            return true;
        }
    }
}
