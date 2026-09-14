namespace Game.Gameplay
{
    public readonly struct DamageArgs
    {
        public IGameEntity Source { get; }
        public int Amount { get; }

        public DamageArgs(IGameEntity source, int amount)
        {
            Source = source;
            Amount = amount;
        }
    }
}
