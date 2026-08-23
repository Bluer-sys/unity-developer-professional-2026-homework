using UnityEngine;

namespace Game
{
    public interface IInteractableComponent
    {
        void Interact(GameObject interactor);
    }
}
