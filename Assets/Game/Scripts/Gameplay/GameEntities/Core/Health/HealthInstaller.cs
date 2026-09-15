using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class HealthInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private int _maxHealth = 10;

        public override void Install(IGameEntity entity)
        {
            entity.AddHealth(new ReactiveVariable<int>(_maxHealth));
            entity.AddMaxHealth(new Const<int>(_maxHealth));
            entity.AddTakeDamageEvent(new Event<DamageArgs>());
            entity.AddDeathEvent(new Event<DamageArgs>());
        }
    }
}
