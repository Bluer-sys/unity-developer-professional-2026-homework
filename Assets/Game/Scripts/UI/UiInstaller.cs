using UnityEngine;
using Zenject;

namespace Game.UI
{
    [CreateAssetMenu(fileName = "UiInstaller", menuName = "Zenject/New UiInstaller")]
    public sealed class UiInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlanetCollectionPresenter>()
                     .FromComponentInHierarchy()
                     .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlanetPresenter>()
                     .FromComponentsInHierarchy()
                     .AsCached();

            Container.BindInterfacesAndSelfTo<PlanetPopupPresenter>()
                     .FromComponentInHierarchy()
                     .AsCached();

            Container.BindInterfacesAndSelfTo<MoneyPresenter>()
                     .FromComponentInHierarchy()
                     .AsCached();
        }
    }
}
