using Newtonsoft.Json.Linq;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class CountdownSerializer : ComponentSerializer<Countdown>
    {
        public override string Key => "countdown";

        protected override JToken SerializeInternal(Countdown component)
        {
            var data = new JObject
            {
                { "current", component.Current }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, Countdown component)
        {
            component.Current = data["current"].Value<int>();

        }
    }
}
