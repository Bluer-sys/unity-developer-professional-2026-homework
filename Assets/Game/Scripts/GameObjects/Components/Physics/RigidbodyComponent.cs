using UnityEngine;

namespace Game
{
    public class RigidbodyComponent
    {
        public Rigidbody2D Rigidbody { get; }

        public RigidbodyComponent(Rigidbody2D rigidbody)
        {
            Rigidbody = rigidbody;
        }
    }
}
