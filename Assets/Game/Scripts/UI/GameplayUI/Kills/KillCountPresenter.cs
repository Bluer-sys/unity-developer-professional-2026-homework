using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public sealed class KillCountPresenter :
        IEntityInit<IPlayerContext>,
        IEntityEnable<IPlayerContext>,
        IEntityDisable<IPlayerContext>
    {
        private readonly KillCountView _view;
        private readonly IGameContext _gameContext;
        private IReactiveValue<int> _killCount;
        private Subscription<int> _subscription;

        public KillCountPresenter(KillCountView view, IGameContext gameContext)
        {
            _view = view;
            _gameContext = gameContext;
        }

        public void Init(IPlayerContext context)
        {
            _killCount = _gameContext.GetKillCount();
        }

        public void Enable(IPlayerContext context)
        {
            _subscription = _killCount.Observe(UpdateView);
        }

        public void Disable(IPlayerContext context)
        {
            _subscription.Dispose();
        }

        private void UpdateView(int killCount)
        {
            _view.SetText(killCount.ToString());
        }
    }
}
