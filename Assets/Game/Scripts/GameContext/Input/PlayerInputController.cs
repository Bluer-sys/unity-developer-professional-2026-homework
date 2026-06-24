using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerInputController : ITickable, IFixedTickable
    {
        private readonly IPlayerProvider _playerProvider;

        public PlayerInputController(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public void Tick()
        {
            var player = _playerProvider.Player;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                var rigidbody2D = player.Get<Rigidbody2D>();
                var jumpComponent = player.Get<JumpComponent>();
                
                jumpComponent.ApplyForce(rigidbody2D);
            }

            if (UnityEngine.Input.GetMouseButtonDown(0))
                player.Get<PushAbilityComponent>().Apply();

            if (UnityEngine.Input.GetMouseButtonDown(1))
                player.Get<BlowUpAbilityComponent>().Apply();
        }

        public void FixedTick()
        {
            MoveTick(Time.fixedDeltaTime);
        }

        private void MoveTick(float deltaTime)
        {
            var player = _playerProvider.Player;
            var horizontal = UnityEngine.Input.GetAxis("Horizontal");
            var direction = new Vector2(horizontal, 0f);

            player.Get<MoveTransformComponent>().Move(direction, deltaTime);

            if (direction.x != 0)
                player.Get<LookComponent>().Look(direction.x);
        }
    }
}
