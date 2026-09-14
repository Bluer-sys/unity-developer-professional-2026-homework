using System;
using Atomic.Elements;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class RootMotionSource : MonoBehaviour, ISignal<Vector3, float>
    {
        [SerializeField] private Animator _animator;

        public event Action<Vector3, float> OnEvent;

        private void OnAnimatorMove()
        {
            OnEvent?.Invoke(_animator.deltaPosition, Time.deltaTime);
        }
    }
}
