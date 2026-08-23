using System;
using Fusion;
using Game.Core;
using Game.GameObjects;
using UnityEngine;

namespace Game
{
    internal class Portal : NetworkBehaviour, IInteractableComponent
    {
        public event Action<Enemy> OnEnemyReached;
        
        [field: SerializeField] public Transform Center { get; private set; }

        public void Interact(GameObject interactor)
        {
            if(interactor.TryGetComponent(out Enemy enemy) &&
               TryGetComponent(out HealthComponent portalHealth))
            {
                OnEnemyReached?.Invoke(enemy);
                portalHealth.Decrement(1);
            }
        }
    }
}
