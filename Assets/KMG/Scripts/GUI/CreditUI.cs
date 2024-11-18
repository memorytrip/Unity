using TMPro;
using UnityEngine;

namespace GUI
{
    public class CreditUI: MonoBehaviour
    {
        
        [SerializeField] private TMP_Text creditText;

        public void SetText(string text)
        {
            creditText.text = text;
        }
    }
}