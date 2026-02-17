using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletInstaller : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private BulletLifetime _bulletLifetime;
        [SerializeField] private BulletBuilder _builder;
        [SerializeField] private MovementComponent _movement;

        public BulletBuilder Install(TransformBounds bounds)
        {
            _movement.Construct(_rigidbody, 0);
            _bulletLifetime.Construct(bounds);

            return _builder;
        }
    }
}
