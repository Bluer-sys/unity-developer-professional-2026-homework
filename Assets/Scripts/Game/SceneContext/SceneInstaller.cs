using Game.GameContext;
using Game.PlayerContext.SnakeContext;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game.SceneContext
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private Coin _coinPrefab;
        
        [Inject] 
        private GameConfig _gameConfig;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ISnakeFacade>()
                .To<SnakeFacade>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<IDifficulty>()
                .To<Difficulty>()
                .AsSingle()
                .WithArguments(_gameConfig.MaxDifficulty);

            Container
                .Bind<IScore>()
                .To<Score>()
                .AsSingle();

            Container
                .Bind<IWorldBounds>()
                .To<WorldBounds>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .BindInterfacesTo<LevelFinishController>()
                .AsCached();
            
            BindCoins();
        }

        private void BindCoins()
        {
            Container
                .BindMemoryPool<Coin, CoinsPool>()
                .WithInitialSize(1)
                .ExpandByOneAtATime()
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransformGroup("Coins")
                .AsSingle();

            Container
                .BindInterfacesTo<CoinsSpawner>()
                .AsCached();
        }
    }
}
