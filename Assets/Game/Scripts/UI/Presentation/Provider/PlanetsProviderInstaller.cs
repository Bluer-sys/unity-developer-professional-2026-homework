using UnityEngine;
using Zenject;

namespace Game.UI.Presentation.Provider
{
    [CreateAssetMenu(fileName = "PlanetsProviderInstaller", menuName = "Zenject/New PlanetsProviderInstaller")]
    public class PlanetsProviderInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlanetsProvider>()
                     .To<PlanetsProvider>()
                     .AsSingle();
        }
    }
}
