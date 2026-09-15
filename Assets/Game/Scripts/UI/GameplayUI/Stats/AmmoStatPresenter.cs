using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public sealed class AmmoStatPresenter :
        IEntityInit<IPlayerContext>,
        IEntityEnable<IPlayerContext>,
        IEntityDisable<IPlayerContext>
    {
        private readonly StatView _view;
        private IReactiveValue<int> _ammo;
        private Subscription<int> _subscription;

        public AmmoStatPresenter(StatView view)
        {
            _view = view;
        }

        public void Init(IPlayerContext context)
        {
            _ammo = context.GetCharacter().GetWeapon().GetAmmo();
        }

        public void Enable(IPlayerContext context)
        {
            _subscription = _ammo.Observe(UpdateView);
        }

        public void Disable(IPlayerContext context)
        {
            _subscription.Dispose();
        }

        private void UpdateView(int ammo)
        {
            _view.SetText(ammo.ToString());
            _view.SetProgress(ammo > 0 ? 1 : 0);
        }
    }
}
