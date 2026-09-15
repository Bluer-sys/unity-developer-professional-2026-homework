using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class CharacterAttackBehaviour :
        IEntityInit<IGameEntity>, 
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>,
        IEntityFixedTick<IGameEntity>
    {
        private IGameEntity _entity;
        private IReactiveValue<bool> _isAiming;
        private IValue<int> _health;
        private ICooldown _delay;
        private IAction _request;

        public void Init(IGameEntity entity)
        {
            _entity = entity;
            _isAiming = entity.GetIsAiming();
            _health = entity.GetHealth();
            _delay = entity.GetFirstAttackDelay();
            _request = entity.GetAttackRequest();
        }

        public void Enable(IGameEntity entity)
        {
            _isAiming.OnEvent += OnAimingChanged;
            _delay.ResetTime();
        }

        public void Disable(IGameEntity entity)
        {
            _isAiming.OnEvent -= OnAimingChanged;
            _delay.ResetTime();
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_health.Value == 0 || !_isAiming.Value)
                return;

            _delay.Tick(deltaTime);
            
            if (_delay.IsCompleted())
                _request.Invoke();
        }

        private void OnAimingChanged(bool isAiming)
        {
            _delay.ResetTime();
            
            if (!isAiming)
                AttackUseCase.Cancel(_entity);
        }
    }
}
