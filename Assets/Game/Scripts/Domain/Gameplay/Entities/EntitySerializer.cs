using System;
using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Gameplay
{
    public class EntitySerializer : ComponentSerializer<Entity>
    {
        public override string Key => "entity";
        
        private readonly IReadOnlyDictionary<Type, IComponentSerializer> _serializers;

        public EntitySerializer(IReadOnlyDictionary<Type, IComponentSerializer> serializers) =>
            _serializers = serializers;

        protected override JToken SerializeInternal(Entity entity)
        {
            var components = entity.GetComponents<Component>();
            var jComponents = new JObject();
            
            foreach(var component in components)
            {
                if(!_serializers.TryGetValue(component.GetType(), out var serializer))
                    continue;
                
                var jToken = serializer.Serialize(component);
                
                jComponents.Add(serializer.Key, jToken);
            }

            var data = new JObject
            {
                { "components", jComponents }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, Entity entity)
        {
            var jComponents = data["components"].Value<JObject>();
            
            var components = entity.GetComponents<Component>();
            
            foreach (Component component in components)
            {
                if (!_serializers.TryGetValue(component.GetType(), out var serializer))
                    continue;
                
                var jComponent = jComponents[serializer.Key];

                serializer.Deserialize(jComponent, component);
            }
        }
    }
}
