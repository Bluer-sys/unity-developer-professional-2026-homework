using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Gameplay
{
    public abstract class ComponentSerializer<TComponent> : ISaveSerializer
        where TComponent : Component
    {
        public abstract string Key { get; }

        public JToken Serialize(object payload)
        {
            if(payload is not TComponent concreteComponent)
                throw new ArgumentException($"Component {payload} is not of type {typeof(TComponent)}");
            
            return SerializeInternal(concreteComponent);
        }

        public void Deserialize(JToken data, object payload)
        {
            if(payload is not TComponent concreteComponent)
                throw new ArgumentException($"Component {payload} is not of type {typeof(TComponent)}");
            
            DeserializeInternal(data, concreteComponent);
        }

        protected abstract JToken SerializeInternal(TComponent component);
        protected abstract void DeserializeInternal(JToken data, TComponent component);
    }
}
