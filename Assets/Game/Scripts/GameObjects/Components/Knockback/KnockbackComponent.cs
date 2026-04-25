using System;
using UnityEngine;

namespace Game
{
    public sealed class KnockbackComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Vector2 Force { get; private set; }

            [field: SerializeField]
            public float Cooldown { get; private set; }
        }

        public event Action OnKnockback;

        private readonly Settings _settings;
        private readonly TransformComponent _transformComponent;

        private float _cooldownEnd;

        public bool IsReady => Time.time >= _cooldownEnd;

        public KnockbackComponent(Settings settings, TransformComponent transformComponent)
        {
            _settings = settings;
            _transformComponent = transformComponent;
        }

        public bool TryKnockback(Collider2D target)
        {
            if (Time.time < _cooldownEnd)
                return false;

            Rigidbody2D rb = target.attachedRigidbody;
            if (rb == null)
                return false;

            Transform self = _transformComponent.Transform;
            if (rb.transform == self)
                return false;

            GameEntity targetEntity = target.GetComponentInParent<GameEntity>();
            if (targetEntity != null
                && targetEntity.TryGet(out PushableComponent pushable)
                && !pushable.CanBePushed)
                return false;

            float dirX = rb.transform.position.x >= self.position.x ? 1f : -1f;
            Vector2 force = new Vector2(_settings.Force.x * dirX, _settings.Force.y);
            rb.AddForce(force, ForceMode2D.Impulse);

            _cooldownEnd = Time.time + _settings.Cooldown;
            OnKnockback?.Invoke();
            return true;
        }
    }
}
