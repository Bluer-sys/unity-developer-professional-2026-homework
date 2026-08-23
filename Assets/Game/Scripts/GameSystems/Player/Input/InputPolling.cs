using Fusion;
using UnityEngine;

namespace Game
{
    public class InputPolling : MonoBehaviour
    {
        [SerializeField] private NetworkEvents _networkEvents;
        [SerializeField] private InputMap _inputMap;

        private InputData _currentInput;
        
        private void Update()
        {
            _currentInput.moveDirection = _inputMap.GetMoveDirection();
        }

        private void OnEnable() =>
            _networkEvents.OnInput.AddListener(OnInput);

        private void OnDisable() =>
            _networkEvents.OnInput.RemoveListener(OnInput);

        private void OnInput(NetworkRunner runner, NetworkInput input)
        {
            input.Set(_currentInput);
        }
    }
}
