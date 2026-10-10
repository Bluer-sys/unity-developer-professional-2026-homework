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
            entity.GetPickupCondition().Add(AmmoUseCase.HasAmmoWeapon);
            entity.AddPickupAction(new InlineAction<IGameEntity>(target => AmmoUseCase.AddAmmo(target, _amount)));
        }
    }
}
