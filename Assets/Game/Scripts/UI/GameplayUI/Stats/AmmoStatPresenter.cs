using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public sealed class AmmoStatPresenter :
        IEntityInit<IGameUIContext>,
        IEntityEnable<IGameUIContext>,
        IEntityDisable<IGameUIContext>
    {
        private readonly StatView _view;
        private readonly IGameContext _gameContext;
        private IReactiveValue<int> _ammo;
        private Subscription<int> _subscription;

        public AmmoStatPresenter(StatView view, IGameContext gameContext)
        {
            _view = view;
            _gameContext = gameContext;
        }

        public void Init(IGameUIContext context)
        {
            _ammo = _gameContext.GetCharacter().GetWeapon().GetAmmo();
        }

        public void Enable(IGameUIContext context)
        {
            _subscription = _ammo.Observe(UpdateView);
        }

        public void Disable(IGameUIContext context)
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
