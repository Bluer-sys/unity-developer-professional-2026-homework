using Atomic.Elements;

namespace Game.Gameplay
{
    public sealed class AmmoConsumptionAction : IAction
    {
        private readonly IVariable<int> _ammo;

        public AmmoConsumptionAction(IVariable<int> ammo)
        {
            _ammo = ammo;
        }

        public void Invoke()
        {
            _ammo.Value--;
        }
    }
}
