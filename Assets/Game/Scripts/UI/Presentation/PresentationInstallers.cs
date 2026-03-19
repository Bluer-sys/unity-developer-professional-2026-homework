using Game.UI.Presentation.Money;
using Game.UI.Presentation.Planet;
using Game.UI.Presentation.Provider;
using Game.UI.Presentation.Signals;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.UI.Presentation
{
    [CreateAssetMenu(fileName = "PresentationInstallers", menuName = "Zenject/New PresentationInstallers")]
    public sealed class PresentationInstallers : ScriptableObjectInstaller
    {
        [Inject] private IPlanetsProvider _planetsProvider;
        
        public override void InstallBindings()
        {
            BindSignalBus();
            BindPlanetPresentation();

            Container
                .BindInterfacesAndSelfTo<MoneyPresentation>()
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<PlanetPopupPresentation>()
                .AsCached();
        }

        private void BindPlanetPresentation()
        {
            Container
                .Bind<IPlanet>()
                .FromMethod(_ => _planetsProvider.NextPlanet())
                .WhenInjectedInto<PlanetPresentation>();

            Container
                .Bind<PlanetPresentation>()
                .AsTransient();
        }

        private void BindSignalBus()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnMoneyEarnedSignal>();
        }
    }
}
