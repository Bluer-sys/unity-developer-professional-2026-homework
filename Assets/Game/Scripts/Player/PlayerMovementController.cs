using Game.Common;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private MovementComponent _movement;
        
        public void Update()
        {
            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            var direction = new Vector2(dx, dy);

            _movement.Move(direction);
        }
    }
}
