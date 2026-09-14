using Atomic.Elements;
using UnityEngine;

namespace Game.Gameplay
{
    public static class DamageUseCase
    {
        public static bool TryTakeDamage(this IGameEntity entity, DamageArgs damage)
        {
            if (damage.Amount <= 0 || !entity.IsAlive())
                return false;

            IVariable<int> health = entity.GetHealth();
            int remainingHealth = Mathf.Max(0, health.Value - damage.Amount);
            health.Value = remainingHealth;

            entity.GetTakeDamageEvent().Invoke(damage);

            if (remainingHealth == 0)
                entity.GetDeathEvent().Invoke(damage);

            return true;
        }
    }
}
