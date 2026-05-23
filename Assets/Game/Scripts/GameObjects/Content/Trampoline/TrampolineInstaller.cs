using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class TrampolineInstaller : MonoInstaller
    {
        [SerializeField] 
        private ForceTargetComponent.Settings _forceSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<TriggerComponent>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<TransformComponent>().AsSingle().WithArguments(transform);
            Container.BindInterfacesAndSelfTo<ForceTargetComponent>().AsSingle().WithArguments(_forceSettings);
            Container.BindInterfacesAndSelfTo<Trampoline>().AsSingle().NonLazy();
        }
    }
}
