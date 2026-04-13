using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace Game.Gameplay
{
    public class DestinationPointSerializer : ComponentSerializer<DestinationPoint>
    {
        public override string Key => "destinationPoint";

        protected override JToken SerializeInternal(DestinationPoint component)
        {
            var data = new JObject
            {
                { "value", JsonConvert.SerializeObject(new SerializedVector3(component.Value)) }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, DestinationPoint component)
        {
            component.Value = JsonConvert.DeserializeObject<SerializedVector3>(
                data["value"].Value<string>());
        }
    }
}
