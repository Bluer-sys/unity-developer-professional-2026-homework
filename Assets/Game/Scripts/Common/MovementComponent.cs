using Modules.Utils;
using UnityEngine;

namespace Game.Common
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private TransformBounds _bounds;
        [SerializeField] private float _speed;
        
        public void Move(Vector2 direction)
        {
            Vector2 delta = direction * (_speed * Time.fixedDeltaTime);
            Vector2 newPosition = _rigidbody.position + delta;

            _rigidbody.MovePosition(newPosition);
            
            if(_bounds != null)
                transform.position = _bounds.ClampInBounds(transform.position);
        }
    }
}
