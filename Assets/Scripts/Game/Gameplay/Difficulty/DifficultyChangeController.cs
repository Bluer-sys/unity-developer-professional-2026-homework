using System;
using Modules;
using Zenject;

namespace Game.Gameplay
{
    public class DifficultyChangeController : IInitializable, IDisposable
    {
        private readonly IDifficulty _difficulty;
        private readonly ICoinsSpawner _coinsSpawner;
        private readonly IGameCycle _gameCycle;

        public DifficultyChangeController(
            IDifficulty difficulty, 
            ICoinsSpawner coinsSpawner,
            IGameCycle gameCycle)
        {
            _difficulty = difficulty;
            _coinsSpawner = coinsSpawner;
            _gameCycle = gameCycle;
        }
        
        public void Initialize()
        {
            _coinsSpawner.OnAllCollected += ChangeDifficulty;
            
            ChangeDifficulty();
        }

        public void Dispose()
        {
            _coinsSpawner.OnAllCollected -= ChangeDifficulty;
        }
        
        private void ChangeDifficulty()
        {
            if (_difficulty.Next(out int difficulty))
            {
                _coinsSpawner.Spawn(difficulty);
                _gameCycle.SetDifficulty(difficulty, _difficulty.Max);
            }
            else
            {
                _gameCycle.SetVictory();
            }
        }
    }
}
