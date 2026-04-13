using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Gameplay
{
    public class ProductionOrderSerializer : ComponentSerializer<ProductionOrder>
    {
        public override string Key => "productionOrder";
        
        private readonly EntityCatalog _entityCatalog;

        public ProductionOrderSerializer(EntityCatalog entityCatalog) =>
            _entityCatalog = entityCatalog;

        protected override JToken SerializeInternal(ProductionOrder component)
        {
            var queue = new JArray();

            foreach (var config in component.Queue)
                queue.Add(config.Name);

            return queue;
        }

        protected override void DeserializeInternal(JToken data, ProductionOrder component)
        {
            var queue = new List<EntityConfig>();

            foreach (var item in data)
            {
                var name = item.Value<string>();

                if (_entityCatalog.FindConfig(name, out var config))
                    queue.Add(config);
                else
                    Debug.LogWarning($"ProductionOrderSerializer: config '{name}' not found in catalog");
            }

            component.Queue = queue;
        }
    }
}
