namespace Game.Utils
{
    public class UnityTimer
    {
        private float _expiredTime;
        private float _lastValue;

        public UnityTimer() {}
        
        public UnityTimer(float value)
        {
            Set(value);
        }

        public void Set(float value)
        {
            _expiredTime = UnityEngine.Time.time + value;
            _lastValue = value;
        }
        
        public void SetRandom(float min, float max)
        {
            Set(UnityEngine.Random.Range(min, max));
        }

        public void Reset()
        {
            Set(_lastValue);
        }

        public bool IsExpired()
        {
            return UnityEngine.Time.time > _expiredTime;
        }
    }
}
