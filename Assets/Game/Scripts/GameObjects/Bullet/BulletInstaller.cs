using Game.GameObjects.Core;
using Modules.Utils;
using UnityEngine;

namespace Game.GameObjects.Bullet
{
    public class BulletInstaller : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CollisionHandler _collisionHandler;
        
        [SerializeField] private Bullet _bullet;
        [SerializeField] private BulletView _bulletView;
        [SerializeField] private BulletBuilder _builder;
        [SerializeField] private MovementComponent _movement;

        public BulletBuilder Install(TransformBounds bounds)
        {
            _builder.Construct(_bullet, _bulletView, _movement);
            _bullet.Construct(bounds, _collisionHandler);
            _bulletView.Construct(_bullet);
            _movement.Construct(_rigidbody, 0);

            return _builder;
        }
    }
}
