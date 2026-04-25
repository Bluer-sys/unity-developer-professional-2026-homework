using Zenject;

namespace Game
{
    public class GameEntity : GameObjectContext, IGameEntity
    {
        public T Get<T>() where T : class
        {
            return this.Container.Resolve<T>();
        }

        public bool TryGet<T>(out T result) where T : class
        {
            result = this.Container.TryResolve<T>();
            return result != null;
        }

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }
    }
}
