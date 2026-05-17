using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class SpiderInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _transform;

        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private MoveComponent.Settings _moveSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private PatrolComponent.Settings _patrolSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;
        [SerializeField] private PushComponent.Settings _pushSettings;
        [SerializeField] private Spider.Settings _spiderSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();

            Container.Bind<TransformComponent>().AsSingle().WithArguments(_transform);
            Container.Bind<RigidbodyComponent>().AsSingle().WithArguments(_rigidbody);
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesAndSelfTo<PatrolComponent>().AsSingle().WithArguments(_patrolSettings);
            Container.BindInterfacesAndSelfTo<PushableComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<PushComponent>().AsSingle().WithArguments(_pushSettings);
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);
            Container.BindInterfacesAndSelfTo<DestroyOnDeathComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<Spider>().AsSingle().WithArguments(_spiderSettings).NonLazy();

            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<MoveViewComponent>().AsSingle();
            Container.BindInterfacesTo<GroundedViewComponent>().AsSingle();
        }
    }
}
