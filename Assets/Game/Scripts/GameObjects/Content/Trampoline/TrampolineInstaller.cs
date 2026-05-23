using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TrampolineInstaller : MonoInstaller
    {
        [SerializeField] private ForceTargetComponent.Settings _forceSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Trampoline>().AsSingle().NonLazy();
            
            Container.Bind<TriggerComponent>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<TransformComponent>().AsSingle().WithArguments(transform);
            Container.BindInterfacesAndSelfTo<ForceTargetComponent>().AsSingle().WithArguments(_forceSettings);
            
            BindView();
        }

        private void BindView()
        {
            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesTo<TrampolineView>().AsSingle();
        }
    }
}
