using Game.Common;
using UnityEngine;

namespace Game.Enemy
{
    public sealed class Enemy : ShipController
    {
        [Header("Enemy")]
        public ShipController target;
        public Vector2 destination;

        [SerializeField]
        private float _fireCooldown = 1.25f;

        [SerializeField]
        private float _stoppingDistance = 0.25f;

        [SerializeField]
        private MovementComponent _movementComponent;
        
        private float _fireTime;

        private IEnemyDespawner _despawner;

        public void SetDespawner(IEnemyDespawner despawner) => _despawner = despawner;

        private void OnEnable() => this.OnDead += this.OnCharacterDead;

        private void OnDisable() => this.OnDead -= this.OnCharacterDead;

        private void OnCharacterDead() => _despawner.Despawn(this);

        protected void FixedUpdate()
        {
            if (this.currentHealth <= 0 || this.target == null || this.target.currentHealth <= 0)
                return;

            Vector2 distance = destination - (Vector2) this.transform.position;
            Vector2 direction = distance.normalized;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            moveDirection = isNotReached ? direction : Vector3.zero;

            if (isNotReached)
            {
                _movementComponent.Move(direction);
            }
            else
            {
                float time = Time.time;
                if (time - _fireTime >= _fireCooldown)
                {
                    this.Fire();
                    _fireTime = time;
                }
            }
        }
    }
}
