using System;
using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public class EntitiesSerializer : ISaveSerializer
    {
        public string Key => "entities";
        
        private readonly EntityWorld _entityWorld;
        private readonly IReadOnlyDictionary<Type, IComponentSerializer> _componentSerializers;

        public EntitiesSerializer(
            EntityWorld entityWorld,
            IReadOnlyDictionary<Type, IComponentSerializer> componentSerializers)
        {
            _entityWorld = entityWorld;
            _componentSerializers = componentSerializers;
        }
        
        public JToken Serialize()
        {
            var jObject = new JObject();
            var entityWorldSerializer = _componentSerializers[typeof(EntityWorld)];

            var jEntityWorld = entityWorldSerializer.Serialize(_entityWorld);
            
            jObject.Add(jEntityWorld);
            
            return jObject;
        }

        public void Deserialize(JToken data)
        {
            var entityWorldSerializer = _componentSerializers[typeof(EntityWorld)];
            
            entityWorldSerializer.Deserialize(data, _entityWorld);
        }
    }
}
