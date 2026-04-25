using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private HealthComponent.Settings _healthSettings;
        [SerializeField] private MoveComponent.Settings _moveSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Character>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TransformComponent>().AsSingle().WithArguments(_transform);
            Container.BindInterfacesAndSelfTo<HealthComponent>().AsSingle().WithArguments(_healthSettings);
            Container.BindInterfacesAndSelfTo<MoveComponent>().AsSingle().WithArguments(_moveSettings);
        }
    }
}
