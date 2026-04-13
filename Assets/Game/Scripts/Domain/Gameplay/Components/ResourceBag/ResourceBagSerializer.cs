using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace Game.Gameplay
{
    public class ResourceBagSerializer : ComponentSerializer<ResourceBag>
    {
        public override string Key => "resourceBag";

        protected override JToken SerializeInternal(ResourceBag component)
        {
            var data = new JObject
            {
                { "type", (int)component.Type },
                { "current", component.Current }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, ResourceBag component)
        {
            component.Type = (ResourceType)data["type"].Value<int>();
            component.Current = data["current"].Value<int>();
        }
    }
}
