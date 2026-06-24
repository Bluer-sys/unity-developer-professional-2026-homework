using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class MonkeyInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        
        [SerializeField] private ForceTargetComponent.Settings _jumpSettings;
        [SerializeField] private CooldownComponent.Settings _jumpCooldownSettings;
        
        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private HealthViewComponent.Settings _healthViewSettings;
        
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private ForceAbilityComponent.Settings _pushSettings;
        [SerializeField] private TargetDetectorComponent.Settings _detectorSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Monkey>().AsSingle().NonLazy();

            Container.BindInstance(transform).AsSingle();
            Container.BindInstance(_rigidbody).AsSingle();
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.Bind<TargetComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetDetectorComponent>().AsSingle().WithArguments(_detectorSettings);
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);
            
            Container.Bind<JumpComponent>().AsSingle().WithArguments(_jumpSettings);
            Container.BindInterfacesAndSelfTo<CooldownComponent>().AsCached().WithArguments(_jumpCooldownSettings);
            
            Container.BindInterfacesAndSelfTo<ForceAbilityComponent>().AsCached().WithArguments(_pushSettings);
            
            BindView();
        }

        private void BindView()
        {
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<GroundedViewComponent>().AsSingle();
            Container.BindInterfacesTo<DeathViewComponent>().AsSingle();
            Container.BindInterfacesTo<HealthViewComponent>().AsSingle().WithArguments(_healthViewSettings);
        }
    }
}
