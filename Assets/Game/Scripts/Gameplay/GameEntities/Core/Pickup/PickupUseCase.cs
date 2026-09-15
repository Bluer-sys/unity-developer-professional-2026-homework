using Atomic.Elements;

namespace Game.Gameplay
{
    public static class PickupUseCase
    {
        public static bool TryCollect(IGameEntity item, IGameEntity target)
        {
            IVariable<bool> isCollected = item.GetIsCollected();

            if (isCollected.Value || !item.GetPickupCondition().Invoke(target))
                return false;

            isCollected.Value = true;
            item.GetPickupAction().Invoke(target);
            item.GetPickupEvent().Invoke();
            return true;
        }
    }
}
