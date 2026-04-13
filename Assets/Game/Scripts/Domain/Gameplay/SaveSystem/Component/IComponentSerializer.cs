using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IComponentSerializer
    {
        string Key { get; }
        
        JToken Serialize(Component component);
        void Deserialize(JToken data, Component component);
    }
}
