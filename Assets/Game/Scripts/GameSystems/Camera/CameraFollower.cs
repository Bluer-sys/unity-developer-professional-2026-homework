using Fusion;
using UnityEngine;

namespace Game.Camera
{
    public class CameraFollower : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _followSpeed;
        
        private PlayerRef _playerRef;
        private CameraTarget _target;

        public void PlayerJoined(PlayerRef player)
        {
            if(Runner.LocalPlayer != player)
                return;
            
            _playerRef = player;
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (Runner.LocalPlayer != player)
                return;
            
            _playerRef = default;
            _target = null;
        }
        
        public override void Render()
        {
            if(!TrySetTarget())
                return;
            
            Vector3 targetPosition = _target.Target.transform.position + _offset;

            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }

        private bool TrySetTarget()
        {
            if(_target != null)
                return true;
            
            if (_playerRef.IsNone)
                return false;

            if (!Runner.TryGetPlayerObject(_playerRef, out var player))
                return false;

            _target = player.GetComponentInChildren<CameraTarget>();

            return _target != null;
        }
    }
}
