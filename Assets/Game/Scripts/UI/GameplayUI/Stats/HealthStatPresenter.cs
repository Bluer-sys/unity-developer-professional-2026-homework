using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public sealed class HealthStatPresenter :
        IEntityInit<IGameUIContext>,
        IEntityEnable<IGameUIContext>,
        IEntityDisable<IGameUIContext>
    {
        private readonly StatView _view;
        private readonly IGameContext _gameContext;
        private IReactiveValue<int> _health;
        private IValue<int> _maxHealth;
        private Subscription<int> _subscription;

        public HealthStatPresenter(StatView view, IGameContext gameContext)
        {
            _view = view;
            _gameContext = gameContext;
        }

        public void Init(IGameUIContext context)
        {
            IGameEntity character = _gameContext.GetCharacter();
            _health = character.GetHealth();
            _maxHealth = character.GetMaxHealth();
        }

        public void Enable(IGameUIContext context)
        {
            _subscription = _health.Observe(UpdateView);
        }

        public void Disable(IGameUIContext context)
        {
            _subscription.Dispose();
        }

        private void UpdateView(int health)
        {
            _view.SetText(health.ToString());
            _view.SetProgress((float)health / _maxHealth.Value);
        }
    }
}
