using Game.Enemy;
using Game.Factory;

namespace Game.Pool
{
    public class EnemyPool : ObjectPool<EnemyFacade, EnemyFactory>
    {
        protected override void Reinitialize(EnemyFacade enemy)
        {
            enemy.ResetHealth();
        }
    }
}
