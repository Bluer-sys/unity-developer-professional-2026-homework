using UnityEngine;

namespace Game.Scripts
{
    public sealed class UnityTimer
    {
        private readonly float _duration;
        
        private float _endTime;

        public UnityTimer(float duration)
        {
            _duration = Mathf.Max(0f, duration);
        }

        public bool IsActive { get; private set; }

        public bool IsFinished => IsActive && Time.time >= _endTime;

        public void Restart()
        {
            _endTime = Time.time + _duration;
            IsActive = true;
        }

        public void Run()
        {
            if (IsActive)
                return;

            Restart();
        }

        public void Stop()
        {
            IsActive = false;
        }
    }
}
