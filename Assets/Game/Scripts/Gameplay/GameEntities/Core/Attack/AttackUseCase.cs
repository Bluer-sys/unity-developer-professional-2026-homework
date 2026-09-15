namespace Game.Gameplay
{
    public static class AttackUseCase
    {
        public static bool TryStart(IGameEntity entity)
        {
            if (!entity.GetAttackDuration().IsCompleted() || !entity.GetAttackCondition().Value)
                return false;

            entity.GetAttackDelay().ResetTime();
            entity.GetAttackDuration().ResetTime();
            entity.GetAttackStartedEvent().Invoke();

            return true;
        }

        public static void Cancel(IGameEntity entity)
        {
            if (entity.GetAttackDuration().IsCompleted())
                return;

            entity.GetAttackDelay().SetTime(0);
            entity.GetAttackDuration().SetTime(0);
            entity.GetAttackCancelledEvent().Invoke();
        }
    }
}
