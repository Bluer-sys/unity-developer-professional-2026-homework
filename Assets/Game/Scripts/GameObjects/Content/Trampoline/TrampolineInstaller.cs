using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TrampolineInstaller : MonoInstaller
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private Trampoline.Settings _trampolineSettings;

        public override void InstallBindings()
        {
            Container.Bind<TriggerComponent>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<TransformComponent>()
                .AsSingle()
                .WithArguments(_transform);
            Container.BindInterfacesAndSelfTo<KnockbackComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<Trampoline>()
                .AsSingle()
                .WithArguments(_trampolineSettings)
                .NonLazy();
        }
    }
}
