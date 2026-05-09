using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class StandingPlatformComponent : IFixedTickable
    {
        private readonly TransformComponent _transformComponent;
        private readonly GroundedComponent _groundedComponent;

        private Transform _currentGround;

        public StandingPlatformComponent(
            TransformComponent transformComponent,
            GroundedComponent groundedComponent)
        {
            _transformComponent = transformComponent;
            _groundedComponent = groundedComponent;
        }

        void IFixedTickable.FixedTick()
        {
            bool standing = _currentGround != null;
            bool hasPlatform = IsStanding(out Transform platform);

            if (!standing && hasPlatform)
            {
                _transformComponent.Transform.parent = platform;
                _currentGround = platform;
            }

            if (standing && !hasPlatform)
            {
                _transformComponent.Transform.parent = null;
                _currentGround = null;
            }
        }

        private bool IsStanding(out Transform platform)
        {
            platform = _groundedComponent.Ground;
            return platform && platform.CompareTag(GameObjectTags.Platform);
        }
    }
}
