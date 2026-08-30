using System;
using Fusion;

namespace Game
{
    public class LifetimeComponent : NetworkBehaviour
    {
        private Action _action;
        private float _lifetime = float.NaN;
        private bool _expired;
        
        [Networked]
        private TickTimer LifetimeTimestamp { get; set; }

        public override void Spawned()
        {
            if(!float.IsNaN(_lifetime))
                ResetTimer();
        }

        public override void FixedUpdateNetwork()
        {
            if (!LifetimeTimestamp.Expired(Runner) || _expired)
                return;

            _action();
            _expired = true;
        }

        public void SetAction(Action action)
        {
            _action = action;
        }

        public void ResetTimer(float lifetime)
        {
            _lifetime = lifetime;
            
            if(Runner != null && Runner.IsRunning)
                ResetTimer();
        }

        private void ResetTimer()
        {
            LifetimeTimestamp = TickTimer.CreateFromSeconds(Runner, _lifetime);
        }
    }
}
