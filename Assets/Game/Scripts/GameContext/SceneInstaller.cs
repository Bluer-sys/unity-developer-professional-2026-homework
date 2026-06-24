using UnityEngine;
using Zenject;

namespace Game
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameEntity _player;
        
        public override void InstallBindings()
        {
            Container.Bind<IPlayerProvider>().To<PlayerProvider>().AsSingle().WithArguments(_player);
            Container.BindInterfacesTo<PlayerInputController>().AsCached().NonLazy();
        }
    }
}
