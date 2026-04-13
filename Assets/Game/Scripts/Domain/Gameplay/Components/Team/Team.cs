using SampleGame.Common;
using UnityEngine;

namespace Game.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }
    }
}
