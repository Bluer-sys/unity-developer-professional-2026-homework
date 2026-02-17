using System;
using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletLifetime : MonoBehaviour
    {
        [SerializeField] private CollisionHandler _collisionHandler;
        
        private TransformBounds _bounds;

        public event Action OnDead;

        public void Construct(TransformBounds bounds)
        {
            _bounds = bounds;
        }
        
        private void OnEnable()
        {
            _collisionHandler.OnTriggerEntered += OnTriggerEnter2D;
        }

        private void OnDisable()
        {
            _collisionHandler.OnTriggerEntered -= OnTriggerEnter2D;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnDead?.Invoke();
        }

        public void FixedUpdate()
        {
            if (!_bounds.InBounds(transform.position))
                OnDead?.Invoke();
        }
    }
}
