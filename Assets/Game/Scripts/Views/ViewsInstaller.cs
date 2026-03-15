using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<PlanetsView>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container
                .Bind<MoneyPanelView>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}
