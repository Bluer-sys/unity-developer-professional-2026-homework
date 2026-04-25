using Zenject;

namespace Game
{
    public sealed class LavaInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TriggerComponent>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<Lava>().AsSingle().NonLazy();
        }
    }
}
