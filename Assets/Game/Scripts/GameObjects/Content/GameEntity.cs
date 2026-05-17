using Zenject;

namespace Game
{
    public class GameEntity : GameObjectContext, IGameEntity, ICoroutineRunner
    {
        public T Get<T>() where T : class
        {
            return Container.Resolve<T>();
        }

        public bool TryGet<T>(out T result) where T : class
        {
            result = Container.TryResolve<T>();
            return result != null;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }
    }
}
