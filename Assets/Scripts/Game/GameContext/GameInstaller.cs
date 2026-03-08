using UnityEngine;
using Zenject;

namespace Game.GameContext
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameConfig)
                     .AsSingle();
        }
    }
}
