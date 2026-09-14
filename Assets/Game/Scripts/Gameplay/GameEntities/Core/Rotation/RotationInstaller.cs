using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class RotationInstaller : MonoEntityInstaller<IGameEntity>
    {
        [SerializeField] private float _rotationSpeed = 360;

        public override void Install(IGameEntity entity)
        {
            entity.AddRotationDirection(new Variable<Vector3>());
            entity.AddRotationSpeed(new Const<float>(_rotationSpeed));
        }
    }
}
