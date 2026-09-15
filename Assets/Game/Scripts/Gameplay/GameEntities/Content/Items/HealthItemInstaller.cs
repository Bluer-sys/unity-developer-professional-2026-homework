using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class HealthItemInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private int _amount = 3;

        public override void Install(IGameEntity entity)
        {
            entity.AddPickupAmount(new Const<int>(_amount));
            entity.GetPickupCondition().Add(target => target.GetHealth().Value < target.GetMaxHealth().Value);
            entity.AddPickupAction(new InlineAction<IGameEntity>(target =>
                target.TryHeal(entity.GetPickupAmount().Value)));
        }
    }
}
