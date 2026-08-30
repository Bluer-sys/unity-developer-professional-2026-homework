using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "TrapCatalog", menuName = "Game/Trap/TrapCatalog")] 
    public class TrapCatalog : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] public Data[] _configs;
        
        public  Dictionary<TrapType, TrapConfig> Configs { get; private  set; } = new();
        
        [Serializable]
        public struct Data
        {
            public TrapType Type;
            public TrapConfig Config;
        }

        public void OnAfterDeserialize()
        {
            foreach (var config in _configs)
                Configs.Add(config.Type, config.Config);
        }

        public void OnBeforeSerialize() {}
    }
}
