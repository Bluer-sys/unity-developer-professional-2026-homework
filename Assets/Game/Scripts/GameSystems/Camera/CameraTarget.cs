using Fusion;
using UnityEngine;

namespace Game.Camera
{
    public class CameraTarget : MonoBehaviour
    {
        [field: SerializeField] public NetworkTransform Target { get; private set; }
    }
}
