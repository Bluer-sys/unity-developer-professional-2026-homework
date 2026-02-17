using Game.GameObjects.Core;
using Game.GameObjects.Weapon;
using UnityEngine;

namespace Game.GameObjects.Ship
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
            Vector2 moveDirection = distance.normalized;
            Vector2 fireDirection = (_target.Transform.position - transform.position).normalized;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            if (isNotReached)
            {
                _movement.SetDirection(moveDirection);
            }
            else
            {
                _movement.ResetDirection();
                _weapon.TryFire(fireDirection);
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
