using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public class HealthSerializer : ComponentSerializer<Health>
    {
        public override string Key => "health";

        protected override JToken SerializeInternal(Health component)
        {
            var data = new JObject
            {
                { "current", component.Current }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, Health component)
        {
            component.Current = data["current"].Value<int>();
        }
    }
}
