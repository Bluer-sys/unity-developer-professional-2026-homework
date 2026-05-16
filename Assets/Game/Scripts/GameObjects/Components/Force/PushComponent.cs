using System;
using Game.Scripts;
using UnityEngine;

namespace Game
{
    public sealed class PushComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public float Cooldown { get; private set; }
        }

        private readonly TransformComponent _transformComponent;
        private readonly ITimer _timer;

        public PushComponent(TransformComponent transformComponent, Settings settings)
        {
            _transformComponent = transformComponent;
            _timer = new UnityTimer(settings.Cooldown);
        }

        public bool TryPush(Rigidbody2D target, Vector2 force)
        {
            if(!_timer.IsFinished)
                return  false;
            
            Transform self = _transformComponent.Transform;
            float dirX = target.transform.position.x >= self.position.x ? 1f : -1f;
            Vector2 finalForce = new Vector2(force.x * dirX, force.y);

            GameEntity targetEntity = target.GetComponentInParent<GameEntity>();

            if (targetEntity != null
                && targetEntity.TryGet(out PushableComponent pushable)
                && !pushable.CanBePushed)
                return false;

            target.AddForce(finalForce, ForceMode2D.Impulse);

            _timer.Restart();
            return true;
        }

        public bool TryPush(Collider2D target, Vector2 force)
        {
            Rigidbody2D rb = target.attachedRigidbody;
            
            if (rb == null)
                return false;
            
            return TryPush(rb, force);
        }
    }
}
