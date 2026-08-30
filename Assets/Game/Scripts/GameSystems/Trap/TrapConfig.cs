using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "TrapConfig", menuName = "Game/Trap/TrapConfig")] 
    public class TrapConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public float Lifetime { get; private set; }
    }
}
