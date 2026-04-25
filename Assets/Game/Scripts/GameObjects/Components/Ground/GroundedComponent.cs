using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GroundedComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Transform Feet { get; private set; }

            [field: SerializeField]
            public LayerMask LayerMask { get; private set; }

            [field: SerializeField]
            public float Distance { get; private set; }
        }

        public event Action<bool> OnGrounded;

        private readonly Settings _settings;

        private Transform _ground;
        private bool _isGrounded;

        public Transform Ground => _ground;
        public bool IsGrounded => _isGrounded;

        public GroundedComponent(Settings settings)
        {
            _settings = settings;
        }

        void IFixedTickable.FixedTick()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                _settings.Feet.position,
                Vector2.down,
                _settings.Distance,
                _settings.LayerMask);

            bool grounded = hit;

            if (grounded != _isGrounded)
            {
                _isGrounded = grounded;
                _ground = _isGrounded ? hit.transform : null;
                OnGrounded?.Invoke(_isGrounded);
            }
            else
            {
                _ground = _isGrounded ? hit.transform : null;
            }
        }
    }
}
