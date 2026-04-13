using Modules.Entities;
using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public class TargetObjectSerializer : ComponentSerializer<TargetObject>
    {
        public override string Key => "targetObject";
        
        private readonly EntityWorld _entityWorld;

        public TargetObjectSerializer(EntityWorld entityWorld) =>
            _entityWorld = entityWorld;

        protected override JToken SerializeInternal(TargetObject component)
        {
            var id = component.Value != null ? component.Value.Id : -1;

            var data = new JObject
            {
                { "id", id }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, TargetObject component)
        {
            var id = data["id"].Value<int>();

            component.Value = id >= 0 && _entityWorld.TryGet(id, out var entity) ? entity : null;
        }
    }
}
