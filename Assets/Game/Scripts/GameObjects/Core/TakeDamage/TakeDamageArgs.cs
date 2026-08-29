using Fusion;

namespace Game
{
    public readonly struct TakeDamageArgs : INetworkStruct
    {
        public readonly int Damage;

        public TakeDamageArgs(int damage)
        {
            Damage = damage;
        }
    }
}
