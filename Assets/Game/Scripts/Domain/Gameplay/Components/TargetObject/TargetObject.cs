using Modules.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    //Can be extended
    public sealed class TargetObject : MonoBehaviour
    {
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }
    }
}
