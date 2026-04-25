using UnityEngine;

namespace Game
{
    public sealed class GroundedComponentDebug : MonoBehaviour
    {
        [SerializeField]
        private Transform _feet;

        [SerializeField]
        private float _distance = 0.15f;

        private void OnDrawGizmos()
        {
            if (_feet == null)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_feet.position, _feet.position + Vector3.down * _distance);
        }
    }
}
