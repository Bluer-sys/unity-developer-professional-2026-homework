using SnakeGame;
using Zenject;

namespace Game.Ui
{
    public class UiInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameUI>()
                .To<GameUI>()
                .FromComponentsInHierarchy()
                .AsSingle()
                .WhenInjectedInto<UiController>();
            
            Container
                .BindInterfacesTo<UiController>()
                .AsCached();
        }
    }
}
