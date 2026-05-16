using UnityEngine;

namespace Game.Scripts
{
    public sealed class UnityTimer : ITimer
    {
        private readonly float _duration;
        
        private float _endTime;

        public UnityTimer(float duration)
        {
            _duration = Mathf.Max(0f, duration);
        }

        public bool IsActive { get; private set; }

        public bool IsFinished => Time.time >= _endTime;

        public void Restart()
        {
            _endTime = Time.time + _duration;
            IsActive = true;
        }

        public void Stop()
        {
            IsActive = false;
        }
    }
}
