using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class StandingPlatformComponent : IFixedTickable
    {
        private readonly Transform _transform;
        private readonly GroundedComponent _groundedComponent;

        private Transform _currentGround;

        public StandingPlatformComponent(
            Transform transform,
            GroundedComponent groundedComponent)
        {
            _transform = transform;
            _groundedComponent = groundedComponent;
        }

        void IFixedTickable.FixedTick()
        {
            bool standing = _currentGround != null;
            bool hasPlatform = IsStanding(out Transform platform);

            if (!standing && hasPlatform)
            {
                _transform.parent = platform;
                _currentGround = platform;
            }

            if (standing && !hasPlatform)
            {
                _transform.parent = null;
                _currentGround = null;
            }
        }

        private bool IsStanding(out Transform platform)
        {
            platform = _groundedComponent.Ground;
            return platform != null && platform.CompareTag(GameObjectTags.Platform);
        }
    }
}
