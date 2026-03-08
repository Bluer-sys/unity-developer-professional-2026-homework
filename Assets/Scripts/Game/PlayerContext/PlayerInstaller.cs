using Game.PlayerContext.Input;
using Game.PlayerContext.SnakeContext;
using Modules;
using Zenject;

namespace Game.PlayerContext
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Install<PlayerInputInstaller>();

            Container
                .BindInterfacesTo<SnakeFacade>()
                .FromComponentInHierarchy()
                .AsCached();

            Container
                .Bind<ISnake>()
                .To<Snake>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container
                .BindInterfacesTo<SnakeHead>()
                .AsCached();
            
            Container
                .BindInterfacesTo<SnakeSpeedController>()
                .AsCached();
        }
    }
}
