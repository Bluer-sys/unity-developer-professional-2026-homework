using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public interface ISaveSerializer
    {
        string Key { get; }
        
        JToken Serialize(object payload = null);
        void Deserialize(JToken data, object payload = null);
    }
}
