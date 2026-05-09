using UnityEngine;

namespace Game
{
    public sealed class LookComponent
    {
        private readonly TransformComponent _transformComponent;

        public float CurrentDirection => _transformComponent.Transform.localScale.x > 0 ? 1 : -1;
        
        public LookComponent(TransformComponent transformComponent)
        {
            _transformComponent = transformComponent;
        }

        public void Look(Transform target)
        {
            Vector2 direction = target.position - _transformComponent.Transform.position;
            Look(direction.x);
        }

        public void Look(float direction)
        {
            _transformComponent.Transform.localScale = new Vector3(direction > 0 ? 1 : -1, 1, 1);
        }
    }
}
