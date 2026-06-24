using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TrapInstaller : MonoInstaller
    {
        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private DamageOnContactComponent.Settings _damageSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Trap>().AsSingle().NonLazy();

            Container.BindInstance(transform).AsSingle();
            Container.Bind<CollisionComponent>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<DamageOnContactComponent>().AsSingle().WithArguments(_damageSettings);
            Container.BindInterfacesAndSelfTo<DestroyOnDeathComponent>().AsSingle();
        }
    }
}
