using Fusion;
using Game.Core;
using UnityEngine;

namespace Game
{
    public class Portal : NetworkBehaviour
    {
        [SerializeField] private GameResult _gameResult;
        
        public override void FixedUpdateNetwork()
        {
            LoseIfDead();
        }

        private void LoseIfDead()
        {
            var healthComponent = GetBehaviour<HealthComponent>();

            if(healthComponent.IsDead)
                _gameResult.Change(GameResult.Result.Lose);
        }
    }
}
