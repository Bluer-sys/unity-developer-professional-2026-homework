using Game.Common;
using UnityEngine;

namespace Game.Player
{
    public class PlayerFireController : MonoBehaviour
    {
        [SerializeField] private AttackComponent _attack;
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _attack.Fire();
        }
    }
}
