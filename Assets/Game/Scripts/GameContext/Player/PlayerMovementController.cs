using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameContext.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private TransformBounds _bounds;
        private MovementComponent _movement;
        
        public void Construct(MovementComponent movement, TransformBounds bounds)
        {
            _movement = movement;
            _bounds = bounds;
        }
        
        public void Update()
        {
            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            var direction = new Vector2(dx, dy);

            _movement.SetDirection(direction);
        }

        public void LateUpdate()
        {
            transform.position = _bounds.ClampInBounds(transform.position);
        }
    }
}
