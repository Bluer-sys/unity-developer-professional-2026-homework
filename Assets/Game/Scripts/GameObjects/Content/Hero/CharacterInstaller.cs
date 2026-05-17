using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private MoveComponent.Settings _moveSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private JumpComponent.Settings _jumpSettings;
        [SerializeField] private PushComponent.Settings _pushSettings;
        [SerializeField] private Character.Settings _characterSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TransformComponent>().AsSingle().WithArguments(transform);
            Container.Bind<RigidbodyComponent>().AsSingle().WithArguments(_rigidbody);
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<PushComponent>().AsSingle().WithArguments(_pushSettings);
            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesAndSelfTo<JumpComponent>().AsSingle().WithArguments(_jumpSettings);
            Container.BindInterfacesTo<DisableRigidbodyOnDeathComponent>().AsSingle();
            Container.BindInterfacesTo<StandingPlatformComponent>().AsSingle();

            Container.BindInterfacesAndSelfTo<Character>().AsSingle().WithArguments(_characterSettings).NonLazy();

            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesTo<MoveViewComponent>().AsSingle();
            Container.BindInterfacesTo<GroundedViewComponent>().AsSingle();
        }
    }
}
