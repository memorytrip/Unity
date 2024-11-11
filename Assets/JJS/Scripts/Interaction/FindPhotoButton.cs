using UnityEngine;
using UnityEngine.UI;

public class FindPhotoButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
}
