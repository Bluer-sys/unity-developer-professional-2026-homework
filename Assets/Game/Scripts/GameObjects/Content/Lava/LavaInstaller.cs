using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class LavaInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntity>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TriggerComponent>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Lava>().AsSingle().NonLazy();
            
            BindView();
        }

        private void BindView()
        {
            Container.Bind<AudioSource>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesTo<LavaView>().AsSingle();
        }
    }
}
