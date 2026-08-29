using Fusion;
using UnityEngine;

namespace Game.Projectiles
{
    public struct Projectile : INetworkStruct
    {
        public int StartTick;
        
        private Vector3Compressed _position;
        private QuaternionCompressed _rotation;
        
        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public Vector3 Direction => (Quaternion) _rotation * Vector3.forward;
        
        public bool IsAlive => StartTick > 0;
    }
}
