using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class MovingPlatformInstaller : MonoInstaller
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private MoveComponent.Settings _moveSettings;
        [SerializeField] private PatrolComponent.Settings _patrolSettings;

        public override void InstallBindings()
        {
            Container.Bind<TransformComponent>().AsSingle().WithArguments(_transform);

            Container.BindInterfacesAndSelfTo<MoveComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<PatrolComponent>().AsSingle().WithArguments(_patrolSettings);

            Container.BindInterfacesAndSelfTo<MovingPlatform>().AsSingle().NonLazy();
        }
    }
}
