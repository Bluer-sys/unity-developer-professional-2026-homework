using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public sealed class HealthStatPresenter :
        IEntityInit<IPlayerContext>,
        IEntityEnable<IPlayerContext>,
        IEntityDisable<IPlayerContext>
    {
        private readonly StatView _view;
        private IReactiveValue<int> _health;
        private IValue<int> _maxHealth;
        private Subscription<int> _subscription;

        public HealthStatPresenter(StatView view)
        {
            _view = view;
        }

        public void Init(IPlayerContext context)
        {
            IGameEntity character = context.GetCharacter();
            _health = character.GetHealth();
            _maxHealth = character.GetMaxHealth();
        }

        public void Enable(IPlayerContext context)
        {
            _subscription = _health.Observe(UpdateView);
        }

        public void Disable(IPlayerContext context)
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
