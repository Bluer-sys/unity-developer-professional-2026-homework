using Game.Presentation.Signals;
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
                .BindInterfacesAndSelfTo<MoneyPresentation>()
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<PlanetPopupPresentation>()
                .AsCached();
            
            Container
                .BindInterfacesAndSelfTo<PlanetsPresentation>()
                .AsCached();
        }

        private void BindSignalBus()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnMoneyEarnedSignal>();
        }
    }
}
