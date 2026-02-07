using Game.Common;
using Game.Interfaces;
using Game.Weapon;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private MovementComponent _movement;
        private IWeapon _weapon;
        private float _stoppingDistance;
        
        private Vector2 _destination;
        private IAttackTarget _target;
        
        public void Construct(MovementComponent movement, IWeapon weapon, float stoppingDistance)
        {
            _movement = movement;
            _weapon = weapon;
            _stoppingDistance = stoppingDistance;
        }
        
        private void FixedUpdate()
        {
            if (_target == null || _target.IsDead)
                return;

            Vector2 distance = _destination - (Vector2)transform.position;
            Vector2 direction = distance.normalized;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            if (isNotReached)
            {
                _movement.Move(direction);
            }
            else
            {
                _weapon.TryFire(_target.Transform.position);
            }
        }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
        }

        public void SetTarget(IAttackTarget target)
        {
            _target = target;
        }
    }
}
