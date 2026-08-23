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
        private NetworkObject _player;

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
            _player = null;
        }
        
        public override void Render()
        {
            if (_playerRef.IsNone)
                return;

            if (_player == null && !Runner.TryGetPlayerObject(_playerRef, out _player))
                return;

            Vector3 targetPosition = _player.transform.position + _offset;

            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }
    }
}
