using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class MovingPlatformInstaller : MonoInstaller
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private MoveTransformComponent.Settings _moveSettings;
        [SerializeField] private PatrolComponent.Settings _patrolSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<TransformComponent>().AsSingle().WithArguments(_transform);

            Container.BindInterfacesAndSelfTo<MoveTransformComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<PatrolComponent>().AsSingle().WithArguments(_patrolSettings);

            Container.BindInterfacesAndSelfTo<MovingPlatform>().AsSingle().NonLazy();
        }
    }
}
