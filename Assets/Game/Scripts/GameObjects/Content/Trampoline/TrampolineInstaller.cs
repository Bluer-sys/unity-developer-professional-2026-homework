using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TrampolineInstaller : MonoInstaller
    {
        [SerializeField]
        private Trampoline.Settings _trampolineSettings;

        public override void InstallBindings()
        {
            Container.Bind<TriggerComponent>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<TransformComponent>().AsSingle().WithArguments(transform);
            Container.BindInterfacesAndSelfTo<PushComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<Trampoline>().AsSingle().WithArguments(_trampolineSettings).NonLazy();
        }
    }
}
