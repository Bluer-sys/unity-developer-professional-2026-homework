using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public interface ISaveSerializer
    {
        string Key { get; }
        
        JToken Serialize();
        void Deserialize(JToken data);
    }
}
