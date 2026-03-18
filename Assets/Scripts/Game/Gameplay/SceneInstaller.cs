using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Coin _coinPrefab;
        
        [Inject] private GameConfig _gameConfig;
        
        public override void InstallBindings()
        {
            Container.Install<PlayerInputInstaller>();
            
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
                .Bind<IGameCycle>()
                .To<GameCycle>()
                .AsSingle();

            BindCoins();
            BindDifficulty();
            BindSnake();
        }

        private void BindDifficulty()
        {
            Container
                .Bind<IDifficulty>()
                .To<Difficulty>()
                .AsSingle()
                .WithArguments(_gameConfig.MaxDifficulty);

            Container
                .BindInterfacesTo<DifficultyChangeController>()
                .AsCached();
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

        private void BindSnake()
        {
            Container
                .Bind<ISnake>()
                .To<Snake>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container
                .BindInterfacesTo<SnakeSpeedController>()
                .AsCached();
            
            Container
                .BindInterfacesTo<SnakeEatingController>()
                .AsCached();
            
            Container
                .BindInterfacesTo<SnakeCollidingController>()
                .AsCached();
        }
    }
}
