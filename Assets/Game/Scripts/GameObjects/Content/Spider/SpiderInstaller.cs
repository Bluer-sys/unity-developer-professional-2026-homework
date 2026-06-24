using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class SpiderInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _transform;
        
        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private HealthViewComponent.Settings _healthViewSettings;

        [SerializeField] private MoveTransformComponent.Settings _moveSettings;
        [SerializeField] private GroundedComponent.Settings _groundedSettings;
        [SerializeField] private ExtraGravityComponent.Settings _gravitySettings;
        [SerializeField] private PatrolComponent.Settings _patrolSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;
        [SerializeField] private ForceTargetComponent.Settings _pushSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Spider>().AsSingle().NonLazy();

            Container.BindInstance(_transform).AsSingle();
            Container.BindInstance(_rigidbody).AsSingle();
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveTransformComponent>().AsSingle().WithArguments(_moveSettings);
            Container.BindInterfacesAndSelfTo<LookComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<GroundedComponent>().AsSingle().WithArguments(_groundedSettings);
            Container.BindInterfacesAndSelfTo<ExtraGravityComponent>().AsSingle().WithArguments(_gravitySettings);
            Container.BindInterfacesAndSelfTo<PatrolComponent>().AsSingle().WithArguments(_patrolSettings);
            Container.BindInterfacesAndSelfTo<ForceTargetComponent>().AsSingle().WithArguments(_pushSettings);
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);

            BindView();
        }

        private void BindView()
        {
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<MoveViewComponent>().AsSingle();
            Container.BindInterfacesTo<GroundedViewComponent>().AsSingle();
            Container.BindInterfacesTo<DeathViewComponent>().AsSingle();
            Container.BindInterfacesTo<HealthViewComponent>().AsSingle().WithArguments(_healthViewSettings);
        }
    }
}
