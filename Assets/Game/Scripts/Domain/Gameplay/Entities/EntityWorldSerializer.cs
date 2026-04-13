using System;
using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace Game.Gameplay
{
    public class EntityWorldSerializer : ISaveSerializer
    {
        public string Key => "entities";

        private readonly EntityWorld _entityWorld;
        private readonly EntityCatalog _entityCatalog;
        private readonly IReadOnlyDictionary<Type, ISaveSerializer> _componentSerializers;

        public EntityWorldSerializer(
                EntityWorld entityWorld,
                EntityCatalog entityCatalog,
                IReadOnlyDictionary<Type, ISaveSerializer> componentSerializers
            )
        {
            _entityWorld = entityWorld;
            _entityCatalog = entityCatalog;
            _componentSerializers = componentSerializers;
        }

        public JToken Serialize(object payload = null)
        {
            var entities = _entityWorld.GetAll();
            var entitySerializer = _componentSerializers[typeof(Entity)];
            var data = new JArray();

            foreach (var entity in entities)
            {
                var jEntity = entitySerializer.Serialize(entity);
                var jObject = jEntity as JObject;

                jObject.Add("id", entity.Id);
                jObject.Add("name", entity.Name);
                jObject.Add("position", JsonConvert.SerializeObject(new SerializedVector3(entity.transform.position)));
                jObject.Add("rotation", JsonConvert.SerializeObject(new SerializedVector3(entity.transform.eulerAngles)));

                data.Add(jEntity);
            }

            return data;
        }

        public void Deserialize(JToken data, object payload = null)
        {
            var entitySerializer = _componentSerializers[typeof(Entity)];
            var createdEntities = new Dictionary<Entity, JToken>();

            _entityWorld.DestroyAll();

            foreach (var jEntity in data)
            {
                var id = jEntity["id"].Value<int>();
                var name = jEntity["name"].Value<string>();
                var position = JsonConvert.DeserializeObject<SerializedVector3>(jEntity["position"].Value<string>());
                var rotation = JsonConvert.DeserializeObject<SerializedVector3>(jEntity["rotation"].Value<string>());

                if (!_entityCatalog.FindConfig(name, out var config))
                {
                    Debug.LogWarning($"Config for name {name} not found");
                    continue;
                }

                var entity = _entityWorld.Spawn(config, position, Quaternion.Euler(rotation), id);
                createdEntities.Add(entity, jEntity);
            }

            foreach (var pair in createdEntities)
                entitySerializer.Deserialize(pair.Value, pair.Key);
        }
    }
}
