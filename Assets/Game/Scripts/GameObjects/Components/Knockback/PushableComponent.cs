namespace Game
{
    public sealed class PushableComponent
    {
        private readonly HealthComponent _healthComponent;
        private readonly GroundedComponent _groundedComponent;

        public bool CanBePushed => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        public PushableComponent(HealthComponent healthComponent, GroundedComponent groundedComponent)
        {
            _healthComponent = healthComponent;
            _groundedComponent = groundedComponent;
        }
    }
}
