using System;
using UnityEngine;

namespace Game.GameObjects.Core
{
    public class CollisionHandler : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEntered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEntered?.Invoke(other);
        }
    }
}
