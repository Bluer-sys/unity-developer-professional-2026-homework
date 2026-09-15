using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class AttackBehaviour : 
        IEntityInit<IGameEntity>, 
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>,
        IEntityFixedTick<IGameEntity>
    {
        private IGameEntity _entity;
        private IRequest _request;
        private IValue<bool> _condition;
        private IAction _action;
        private ICooldown _delay;
        private ICooldown _duration;
        private ISignal<DamageArgs> _deathEvent;

        public void Init(IGameEntity entity)
        {
            _entity = entity;
            _request = entity.GetAttackRequest();
            _condition = entity.GetAttackCondition();
            _action = entity.GetAttackAction();
            _delay = entity.GetAttackDelay();
            _duration = entity.GetAttackDuration();
            _deathEvent = entity.GetDeathEvent();
        }

        public void Enable(IGameEntity entity)
        {
            _delay.OnCompleted += OnDelayCompleted;
            _deathEvent.OnEvent += OnDeath;
        }

        public void Disable(IGameEntity entity)
        {
            _delay.OnCompleted -= OnDelayCompleted;
            _deathEvent.OnEvent -= OnDeath;
            
            _request.Consume();
            AttackUseCase.Cancel(entity);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            UpdateAttack(entity, deltaTime);

            if (_request.Consume() &&
                AttackUseCase.TryStart(entity) &&
                _delay.IsCompleted())
                OnDelayCompleted();
        }

        private void UpdateAttack(IGameEntity entity, float deltaTime)
        {
            if (_duration.IsCompleted())
                return;

            if (!_delay.IsCompleted() && !_condition.Value)
            {
                AttackUseCase.Cancel(entity);
                return;
            }

            _delay.Tick(deltaTime);
            _duration.Tick(deltaTime);
        }

        private void OnDelayCompleted()
        {
            if (_duration.IsCompleted())
                return;

            if (!_condition.Value)
            {
                AttackUseCase.Cancel(_entity);
                return;
            }

            _action.Invoke();
        }

        private void OnDeath(DamageArgs damage)
        {
            _request.Consume();
            AttackUseCase.Cancel(_entity);
        }
    }
}
