using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PickupBehaviour :
        IEntityInit<IGameEntity>,
        IEntityEnable<IGameEntity>,
        IEntityDisable<IGameEntity>
    {
        private readonly TriggerEvents _triggerEvents;
        private IGameEntity _entity;

        public PickupBehaviour(TriggerEvents triggerEvents)
        {
            _triggerEvents = triggerEvents;
        }

        public void Init(IGameEntity entity)
        {
            _entity = entity;
        }

        public void Enable(IGameEntity entity)
        {
            _triggerEvents.OnEntered += OnContact;
            _triggerEvents.OnStay += OnContact;
        }

        public void Disable(IGameEntity entity)
        {
            _triggerEvents.OnEntered -= OnContact;
            _triggerEvents.OnStay -= OnContact;
        }

        private void OnContact(Collider collider)
        {
            GameEntity target = collider.GetComponentInParent<GameEntity>();

            if (target != null)
                PickupUseCase.TryCollect(_entity, target);
        }
    }
}
