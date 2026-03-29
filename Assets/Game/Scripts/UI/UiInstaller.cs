using UnityEngine;
using Zenject;

namespace Game.UI
{
    [CreateAssetMenu(fileName = "UiInstaller", menuName = "Zenject/New UiInstaller")]
    public sealed class UiInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            BindSignalBus();
            
            Container.Bind<PlanetCollectionPresenter>()
                     .FromComponentInHierarchy()
                     .AsSingle()
                     .NonLazy();
            
            Container.BindInterfacesAndSelfTo<PlanetPresenter>()
                     .FromComponentsInHierarchy()
                     .AsCached()
                     .NonLazy();

            Container.BindInterfacesTo<PlanetPopupPresenter>()
                     .FromComponentInHierarchy()
                     .AsSingle()
                     .NonLazy();

            Container.BindInterfacesTo<MoneyPresenter>()
                     .FromComponentInHierarchy()
                     .AsSingle()
                     .NonLazy();
        }

        private void BindSignalBus()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnMoneyEarnedSignal>();
            Container.DeclareSignal<OnPlanetPopupRequestedSignal>();
        }
    }
}
