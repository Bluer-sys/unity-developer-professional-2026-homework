using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [CreateAssetMenu(fileName = "PresentationInstallers", menuName = "Zenject/New PresentationInstallers")]
    public sealed class PresentationInstallers : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            BindSignalBus();

            Container
                .BindInterfacesAndSelfTo<MoneyPresenter>()
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<PlanetPopupPresenter>()
                .AsCached();
            
            Container
                .BindInterfacesAndSelfTo<PlanetCollectionPresentation>()
                .AsCached();
        }

        private void BindSignalBus()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnMoneyEarnedSignal>();
            Container.DeclareSignal<OnPlanetPopupRequestedSignal>();
        }
    }
}
