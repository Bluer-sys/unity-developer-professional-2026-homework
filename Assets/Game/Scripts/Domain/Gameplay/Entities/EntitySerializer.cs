using System;
using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
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
            var jComponents = new JArray(components.Length);
            
            foreach(var component in components)
            {
                var serializer = _serializers[component.GetType()];
                var jToken = serializer.Serialize(component);
                
                jComponents.Add(jToken);
            }

            var data = new JObject
            {
                { "position", new JObject(new SerializedVector3(entity.transform.position)) },
                { "rotation", new JObject(new SerializedVector3(entity.transform.eulerAngles)) },
                { "id", entity.Id },
                { "name", entity.Name },
                { "components", jComponents }
            };

            return data;
        }

        protected override void DeserializeInternal(JToken data, Entity entity)
        {
            var jComponents = data["components"].Value<JArray>();
            
            var components = entity.GetComponents<Component>();
            
            foreach (Component component in components)
            {
                var serializer = _serializers[component.GetType()];
                var jComponent = jComponents[serializer.Key];

                serializer.Deserialize(jComponent, component);
            }
        }
    }
}
