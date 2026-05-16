namespace Game.Scripts
{
    public interface ITimer
    {
        bool IsActive { get; }
        bool IsFinished { get; }

        void Restart();
        void Stop();
    }
}
