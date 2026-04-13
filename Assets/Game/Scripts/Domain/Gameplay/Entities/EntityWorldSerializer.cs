using System;
using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace Game.Gameplay
{
    public class EntityWorldSerializer : ComponentSerializer<EntityWorld>
    {
        public override string Key => "entityWorld";

        private readonly IReadOnlyDictionary<Type, IComponentSerializer> _serializers;
        private readonly EntityCatalog _entityCatalog;

        public EntityWorldSerializer(
            IReadOnlyDictionary<Type, IComponentSerializer> serializers,
            EntityCatalog entityCatalog)
        {
            _serializers = serializers;
            _entityCatalog = entityCatalog;
        }

        protected override JToken SerializeInternal(EntityWorld entityWorld)
        {
            var entities = entityWorld.GetAll();
            var entitySerializer = _serializers[typeof(Entity)];
            var data = new JArray();

            foreach (var entity in entities)
            {
                var jEntity = entitySerializer.Serialize(entity);
                
                data.Add(jEntity);
            }
            
            return data;
        }

        protected override void DeserializeInternal(JToken data, EntityWorld entityWorld)
        {
            entityWorld.DestroyAll();
            
            foreach (var jEntity in data)
            {
                var id = jEntity["id"].Value<int>();
                var name = jEntity["name"].Value<string>();
                var position = jEntity["position"].Value<SerializedVector3>();
                var rotation = jEntity["rotation"].Value<SerializedVector3>();
                
                if(!_entityCatalog.FindConfig(name, out var config))
                {
                    Debug.LogWarning($"Config for name {name} not found");
                    continue;
                }
                
                entityWorld.Spawn(config, position, Quaternion.Euler(rotation), id);
            }
        }
    }
}
