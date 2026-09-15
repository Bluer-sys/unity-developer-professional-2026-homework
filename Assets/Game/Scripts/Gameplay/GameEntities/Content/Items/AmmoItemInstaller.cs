using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class AmmoItemInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private int _amount = 10;

        public override void Install(IGameEntity entity)
        {
            entity.AddPickupAmount(new Const<int>(_amount));
            entity.GetPickupCondition().Add(target => AmmoUseCase.HasAmmoWeapon(target));
            entity.AddPickupAction(new InlineAction<IGameEntity>(target =>
                AmmoUseCase.AddAmmo(target, entity.GetPickupAmount().Value)));
        }
    }
}
