using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class SnakeInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private MoveTransformComponent.Settings _moveSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private TargetDetectorComponent.Settings _detectorSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;
        [SerializeField] private ForceTargetComponent.Settings _pushSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<TransformComponent>().AsSingle().WithArguments(transform);
            Container.Bind<RigidbodyComponent>().AsSingle().WithArguments(_rigidbody);
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveTransformComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesAndSelfTo<TargetDetectorComponent>().AsSingle().WithArguments(_detectorSettings);
            Container.BindInterfacesAndSelfTo<ForceTargetComponent>().AsSingle().WithArguments(_pushSettings);
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);
            Container.BindInterfacesAndSelfTo<DestroyOnDeathComponent>().AsSingle();

            Container.BindInterfacesAndSelfTo<Snake>().AsSingle().NonLazy();
        }
    }
}
