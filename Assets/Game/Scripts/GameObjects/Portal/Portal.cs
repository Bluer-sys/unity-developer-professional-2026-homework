using Fusion;
using Game.GameObjects;
using UnityEngine;

namespace Game
{
    internal class Portal : NetworkBehaviour, IInteractableComponent
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        
        [field: SerializeField] public Transform Center { get; private set; }

        public void Interact(GameObject interactor)
        {
            if(interactor.TryGetComponent(out Enemy enemy))
                _enemySpawner.Despawn(enemy);
        }
    }
}
