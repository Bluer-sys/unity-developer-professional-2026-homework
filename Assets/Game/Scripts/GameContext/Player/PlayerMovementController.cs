using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameContext.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private TransformBounds _bounds;
        [SerializeField] private MovementComponent _movement;
        [SerializeField] private Transform _playerTransform;
        
        public void Update()
        {
            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            var direction = new Vector2(dx, dy);

            _movement.SetDirection(direction);
        }

        public void LateUpdate()
        {
            _playerTransform.position = _bounds.ClampInBounds(_playerTransform.position);
        }
    }
}
