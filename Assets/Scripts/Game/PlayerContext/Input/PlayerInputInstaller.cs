using Zenject;

namespace Game.PlayerContext.Input
{
    public class PlayerInputInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerInput>()
                     .AsCached();

            Container.BindInterfacesTo<PlayerInputController>()
                     .AsCached();
        }
    }
}
