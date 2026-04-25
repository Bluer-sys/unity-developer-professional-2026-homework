using Zenject;

namespace Game
{
    public sealed class Character : IInitializable, MoveComponent.ICondition
    {
        private readonly HealthComponent _healthComponent;
        private readonly MoveComponent _moveComponent;

        public Character(HealthComponent healthComponent, MoveComponent moveComponent)
        {
            _healthComponent = healthComponent;
            _moveComponent = moveComponent;
        }

        void IInitializable.Initialize()
        {
            _moveComponent.SetCondition(this);
        }

        bool MoveComponent.ICondition.Evaluate()
        {
            return _healthComponent.IsAlive;
        }
    }
}
