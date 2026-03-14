using UnityEngine;
using Zenject;

namespace Game.Presentation
{
    [CreateAssetMenu(fileName = "PresentationInstallers", menuName = "Zenject/New PresentationInstallers")]
    public sealed class PresentationInstallers : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<MoneyPanelPresentation>()
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<PlanetPopupPresentation>()
                .AsCached();
            
            Container
                .BindInterfacesAndSelfTo<PlanetsPresentation>()
                .AsCached();
        }
    }
}
