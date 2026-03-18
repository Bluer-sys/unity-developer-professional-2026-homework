using Zenject;

namespace Game.Gameplay
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
