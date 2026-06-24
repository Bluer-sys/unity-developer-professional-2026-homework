using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        
        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private HealthViewComponent.Settings _healthViewSettings;
        
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
            
            Container.BindInstance(transform).AsSingle();
            Container.BindInstance(_rigidbody).AsSingle();
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveTransformComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesTo<StandingPlatformComponent>().AsSingle();
            
            Container.Bind<JumpComponent>().AsSingle().WithArguments(_jumpSettings);
            Container.Bind<PushAbilityComponent>().AsSingle().WithArguments(_pushSettings);
            Container.Bind<BlowUpAbilityComponent>().AsSingle().WithArguments(_blowUpSettings);

            BindView();
        }

        private void BindView()
        {
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesTo<MoveViewComponent>().AsSingle();
            Container.BindInterfacesTo<HealthViewComponent>().AsSingle().WithArguments(_healthViewSettings);
            Container.BindInterfacesTo<DeathViewComponent>().AsSingle();
            Container.BindInterfacesTo<GroundedViewComponent>().AsSingle();
        }
    }
}
