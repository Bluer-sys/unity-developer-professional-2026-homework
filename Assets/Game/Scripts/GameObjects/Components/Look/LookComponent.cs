using UnityEngine;

namespace Game
{
    public sealed class LookComponent
    {
        private readonly TransformComponent _transformComponent;

        public LookComponent(TransformComponent transformComponent)
        {
            _transformComponent = transformComponent;
        }

        public void Look(Transform target)
        {
            Vector2 direction = target.position - _transformComponent.Transform.position;
            this.Look(direction.x);
        }

        public void Look(float direction)
        {
            float angle = direction > 0 ? 0 : 180;
            _transformComponent.Transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}
