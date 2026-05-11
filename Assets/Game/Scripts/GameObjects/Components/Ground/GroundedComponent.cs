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

        public Transform Ground { get; private set; }
        public bool IsGrounded { get; private set; }

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

            bool isGrounded = hit.collider != null;

            if (IsGrounded != isGrounded)
            {
                IsGrounded = isGrounded;
                OnGrounded?.Invoke(IsGrounded);
            }
            
            Ground = IsGrounded ? hit.transform : null;
        }
    }
}
