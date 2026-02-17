using Game.GameContext.Core;
using Game.GameObjects.Ship;

namespace Game.GameContext.Enemy
{
    public class EnemyPool : ObjectPool<EnemyFacade, EnemyFactory>
    {
        protected override void Reinitialize(EnemyFacade enemy)
        {
            enemy.ResetHealth();
        }
    }
}
