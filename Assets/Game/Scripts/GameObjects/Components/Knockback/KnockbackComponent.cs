using UnityEngine;

namespace Game
{
    public sealed class KnockbackComponent
    {
        private readonly TransformComponent _transformComponent;

        public KnockbackComponent(TransformComponent transformComponent)
        {
            _transformComponent = transformComponent;
        }

        public bool TryKnockback(Collider2D target, Vector2 force)
        {
            Rigidbody2D rb = target.attachedRigidbody;
            
            if (rb == null)
                return false;
            
            Transform self = _transformComponent.Transform;
            float dirX = rb.transform.position.x >= self.position.x ? 1f : -1f;
            Vector2 finalForce = new Vector2(force.x * dirX, force.y);

            GameEntity targetEntity = target.GetComponentInParent<GameEntity>();
            
            if (targetEntity != null
                && targetEntity.TryGet(out PushableComponent pushable)
                && !pushable.CanBePushed)
                return false;
            
            rb.AddForce(finalForce, ForceMode2D.Impulse);
            
            return true;
        }
    }
}
