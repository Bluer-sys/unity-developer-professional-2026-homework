using Game.GameContext.Core;

namespace Game.GameContext.Enemy
{
    public class EnemyPool : ObjectPool<GameObjects.Ship.Enemy, EnemyFactory>
    {
        protected override void Reinitialize(GameObjects.Ship.Enemy enemy)
        {
            enemy.ResetHealth();
        }
    }
}
