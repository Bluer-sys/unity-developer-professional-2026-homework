using Game.Data;

namespace Game.Interfaces
{
    internal interface IDamageable
    {
        TeamType Team { get; }
        void TakeDamage(int damage);
    }
}
