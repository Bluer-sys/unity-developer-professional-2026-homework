using UnityEngine;

namespace Game.Gameplay
{
    //Can be extended
    public sealed class Damage : MonoBehaviour
    {
        ///Const
        [field: SerializeField]
        public int Value { get; private set; } = 10;
    }
}
