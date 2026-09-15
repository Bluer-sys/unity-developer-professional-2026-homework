using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class EnemyKillCountBehaviour :
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private readonly IGameContext _gameContext;
        private IVariable<int> _killCount;
        private ISignal<DamageArgs> _deathEvent;

        public EnemyKillCountBehaviour(IGameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void Init(IGameEntity entity)
        {
            _killCount = _gameContext.GetKillCount();
            _deathEvent = entity.GetDeathEvent();
        }

        public void Enable(IGameEntity entity)
        {
            _deathEvent.OnEvent += OnDeath;
        }

        public void Disable(IGameEntity entity)
        {
            _deathEvent.OnEvent -= OnDeath;
        }

        private void OnDeath(DamageArgs damage)
        {
            if (damage.Source.HasPlayerTag())
                _killCount.Value++;
        }
    }
}
