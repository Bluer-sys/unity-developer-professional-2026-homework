using System;
using UnityEngine;

namespace Game
{
    public class CooldownComponent
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField] 
            public float Cooldown { get; private set; }
        }

        private readonly Settings _settings;

        private float _currentTime;

        public bool IsExpired => Time.time - _currentTime >= _settings.Cooldown;

        public CooldownComponent(Settings settings)
        {
            _settings = settings;
        }

        public void Reset()
        {
            _currentTime = Time.time;
        }
    }
}
