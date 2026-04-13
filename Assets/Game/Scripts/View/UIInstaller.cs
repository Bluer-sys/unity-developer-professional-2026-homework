using Game.Gameplay;
using Zenject;

namespace Game.View
{
    //Don't modify
    public sealed class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<ControlsView>().FromComponentInHierarchy().AsSingle();
            this.Container.Bind<IControlsPresenter>().To<ControlsPresenter>().AsSingle();
        }
    }
}
