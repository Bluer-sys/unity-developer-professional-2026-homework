using System;
using Game.PlayerContext.SnakeContext;
using Zenject;

namespace Game.SceneContext
{
    public class LevelFinishController : IInitializable, IDisposable
    {
        private readonly ISnakeFacade _snakeFacade;
        private readonly ICoinsSpawner _coinsSpawner;

        public LevelFinishController(ISnakeFacade snakeFacade, ICoinsSpawner coinsSpawner)
        {
            _snakeFacade = snakeFacade;
            _coinsSpawner = coinsSpawner;
        }
        
        public void Initialize()
        {
            _coinsSpawner.OnCoinsOver += OnLevelFinished;
            _snakeFacade.OnCollided += OnLevelFinished;
        }

        public void Dispose()
        {
            _coinsSpawner.OnCoinsOver -= OnLevelFinished;
            _snakeFacade.OnCollided -= OnLevelFinished;
        }

        private void OnLevelFinished()
        {
            _snakeFacade.SetActive(false);
        }
    }
}
