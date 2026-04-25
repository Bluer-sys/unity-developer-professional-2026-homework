using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class ChaseComponent : IFixedTickable
    {
        private readonly TransformComponent _transformComponent;
        private readonly MoveComponent _moveComponent;

        private Transform _target;
        private bool _enabled;

        public ChaseComponent(TransformComponent transformComponent, MoveComponent moveComponent)
        {
            _transformComponent = transformComponent;
            _moveComponent = moveComponent;
        }

        public void SetTarget(Transform target) => _target = target;
        public void Enable() => _enabled = true;
        public void Disable() => _enabled = false;

        void IFixedTickable.FixedTick()
        {
            if (!_enabled)
                return;

            if (_target == null)
                return;

            float dirX = (_target.position.x > _transformComponent.Transform.position.x) ? 1f : -1f;
            _moveComponent.Move(new Vector2(dirX, 0));
        }
    }
}
