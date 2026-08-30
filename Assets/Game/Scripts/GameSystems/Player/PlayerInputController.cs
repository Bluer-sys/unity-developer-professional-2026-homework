using Fusion;
using Game.Core;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInputController : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _character;

        [Networked]
        private NetworkButtons PreviousButtons { get; set; }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out InputData inputData))
            {
                NetworkButtons inputButtons = inputData.buttons;
                ProcessMove(inputData.moveDirection);
                ProcessTurret(inputButtons);
                ProcessMine(inputButtons);
                PreviousButtons = inputButtons;
            }
            else
            {
                StopMove();
            }
        }

        private void ProcessTurret(NetworkButtons inputButtons)
        {
            if (inputButtons.WasPressed(PreviousButtons, PlayerKeys.Turret))
                Runner.GetBehaviour<TrapShop>().TryBuy(TrapType.Turret, _character);
        }

        private void ProcessMine(NetworkButtons inputButtons)
        {
            if (inputButtons.WasPressed(PreviousButtons, PlayerKeys.Mine))
                Runner.GetBehaviour<TrapShop>().TryBuy(TrapType.Mine, _character);
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
