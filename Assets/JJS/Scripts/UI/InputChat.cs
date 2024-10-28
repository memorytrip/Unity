using TMPro;
using UnityEngine;

public class InputChat : MonoBehaviour
{
    private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.enabled = false;
        _inputField.interactable = false;
    }
}
