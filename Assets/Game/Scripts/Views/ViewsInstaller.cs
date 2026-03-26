using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<PlanetView>()
                .FromComponentsInHierarchy()
                .AsCached();
        }
    }
}
