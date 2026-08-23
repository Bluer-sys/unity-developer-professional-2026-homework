using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInputController : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _character;

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out InputData input))
                ProcessMove(input.moveDirection);
            else
                StopMove();
        }

        private void ProcessMove(Vector2 inputDirection)
        {
            MoveComponent moveComponent = _character.GetBehaviour<MoveComponent>();
            Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
            moveComponent.Move(moveDirection);
        }

        private void StopMove()
        {
            _character.GetBehaviour<MoveComponent>().Stop();
        }
    }
}
