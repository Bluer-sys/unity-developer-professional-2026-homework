using System;
using UnityEngine;
using Zenject;

namespace Game.GameContext
{
    public class PlayerInput : ITickable, IPlayerInput
    {
        public event Action<float> OnHorizontalAxis;

        public event Action<float> OnVerticalAxis;

        public void Tick()
        {
            float axisX = UnityEngine.Input.GetAxis("Horizontal");
            float axisY = UnityEngine.Input.GetAxis("Vertical");

            if(!Mathf.Approximately(axisX, 0.0f))
                OnHorizontalAxis?.Invoke(axisX);

            if (!Mathf.Approximately(axisY, 0.0f))
                OnVerticalAxis?.Invoke(axisY);
        }
    }
}
