using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace Game.Gameplay
{
    public class TeamSerializer : ComponentSerializer<Team>
    {
        public override string Key => "team";

        protected override JToken SerializeInternal(Team component)
        {
            var data = new JObject
            {
                { "type", (int)component.Type }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, Team component)
        {
            component.Type = (TeamType)data["type"].Value<int>();
        }
    }
}
