using Game.Bullet;
using Game.Common;
using Modules.Utils;
using UnityEngine;

namespace Game.Installers
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
