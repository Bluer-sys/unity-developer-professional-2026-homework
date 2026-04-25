using UnityEngine;

namespace Game
{
    public class TransformComponent
    {
        public Transform Transform { get; }

        public TransformComponent(Transform transform)
        {
            Transform = transform;
        }
    }
}
