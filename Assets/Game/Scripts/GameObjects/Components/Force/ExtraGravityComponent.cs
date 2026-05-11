using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class ExtraGravityComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Gravity { get; private set; }
        }

        private readonly Settings _settings;
        private readonly GroundedComponent _groundedComponent;
        private readonly RigidbodyComponent _rigidbodyComponent;

        public ExtraGravityComponent(
            Settings settings,
            GroundedComponent groundedComponent,
            RigidbodyComponent rigidbodyComponent)
        {
            _settings = settings;
            _groundedComponent = groundedComponent;
            _rigidbodyComponent = rigidbodyComponent;
        }

        void IFixedTickable.FixedTick()
        {
            if (_groundedComponent.IsGrounded)
                return;

            Rigidbody2D rigidbody = _rigidbodyComponent.Rigidbody;
            rigidbody.linearVelocity += new Vector2(0, _settings.Gravity * Time.fixedDeltaTime);
        }
    }
}
