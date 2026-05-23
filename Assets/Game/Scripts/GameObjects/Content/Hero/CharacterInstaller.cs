using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private MoveTransformComponent.Settings _moveSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private ForceTargetComponent.Settings _jumpSettings;
        [SerializeField] private ForceAbilityComponent.Settings _pushSettings;
        [SerializeField] private ForceAbilityComponent.Settings _blowUpSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Character>().AsSingle().NonLazy();
            
            Container.Bind<TransformComponent>().AsSingle().WithArguments(transform);
            Container.Bind<RigidbodyComponent>().AsSingle().WithArguments(_rigidbody);
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveTransformComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesTo<DisableRigidbodyOnDeathComponent>().AsSingle();
            Container.BindInterfacesTo<StandingPlatformComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ForceTargetComponent>().AsCached().WithArguments(_jumpSettings);
            Container.BindInterfacesAndSelfTo<ForceAbilityComponent>().AsCached().WithConcreteId("Push").WithArguments(_pushSettings);
            Container.BindInterfacesAndSelfTo<ForceAbilityComponent>().AsCached().WithConcreteId("BlowUp").WithArguments(_blowUpSettings);

            // View
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesTo<MoveViewComponent>().AsSingle();
            Container.BindInterfacesTo<GroundedViewComponent>().AsSingle();
        }
    }
}
