using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class SnakeInstaller : MonoInstaller
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private MoveComponent.Settings _moveSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private TargetDetectorComponent.Settings _detectorSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;
        [SerializeField] private Snake.Settings _snakeSettings;

        public override void InstallBindings()
        {
            Container.Bind<TransformComponent>().AsSingle().WithArguments(_transform);
            Container.Bind<RigidbodyComponent>().AsSingle().WithArguments(_rigidbody);
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesAndSelfTo<TargetDetectorComponent>().AsSingle().WithArguments(_detectorSettings);
            Container.BindInterfacesAndSelfTo<ChaseComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<KnockbackComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);
            Container.BindInterfacesAndSelfTo<DestroyOnDeathComponent>().AsSingle();

            Container.BindInterfacesAndSelfTo<Snake>().AsSingle().WithArguments(_snakeSettings).NonLazy();
        }
    }
}
