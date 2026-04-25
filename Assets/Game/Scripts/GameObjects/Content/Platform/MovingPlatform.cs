using Zenject;

namespace Game
{
    public sealed class MovingPlatform : IInitializable
    {
        private readonly PatrolComponent _patrolComponent;

        public MovingPlatform(PatrolComponent patrolComponent)
        {
            _patrolComponent = patrolComponent;
        }

        void IInitializable.Initialize() =>
            _patrolComponent.Enable();
    }
}
