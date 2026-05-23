using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class MonkeyInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private ForceTargetComponent.Settings _jumpSettings;
        [SerializeField] private ForceAbilityComponent.Settings _pushSettings;
        [SerializeField] private TargetDetectorComponent.Settings _detectorSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<TransformComponent>().AsSingle().WithArguments(transform);
            Container.Bind<RigidbodyComponent>().AsSingle().WithArguments(_rigidbody);
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesAndSelfTo<TargetDetectorComponent>().AsSingle().WithArguments(_detectorSettings);
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);
            Container.BindInterfacesAndSelfTo<DestroyOnDeathComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ForceTargetComponent>().AsCached().WithArguments(_jumpSettings);
            Container.BindInterfacesAndSelfTo<ForceAbilityComponent>().AsCached().WithArguments(_pushSettings);
            
            Container.BindInterfacesAndSelfTo<Monkey>().AsSingle().NonLazy();
        }
    }
}
