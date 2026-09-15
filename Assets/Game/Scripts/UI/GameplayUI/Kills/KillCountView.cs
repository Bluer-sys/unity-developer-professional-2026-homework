using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class KillCountView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;

        public void SetText(string value)
        {
            _valueText.text = value;
        }
    }
}
