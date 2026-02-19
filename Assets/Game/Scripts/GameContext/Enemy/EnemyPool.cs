using Game.GameObjects;

namespace Game.GameContext
{
    public class EnemyPool : ObjectPool<Enemy, EnemyFactory>
    {
        protected override void Reinitialize(Enemy enemy)
        {
            enemy.ResetHealth();
        }
    }
}
