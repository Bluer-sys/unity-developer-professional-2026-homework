using Game.GameObjects.Core;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletCollideEffectController : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private CollisionHandler _collisionHandler;

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
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
