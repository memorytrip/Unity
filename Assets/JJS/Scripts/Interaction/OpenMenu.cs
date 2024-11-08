using Fusion;
using GUI;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour, IClickable3dObject
{
    [SerializeField] private FadeController fader;
    [SerializeField] private Button confirmButton;
    private const float FadeDuration = 0.5f;
    
    public void OnClick()
    {
        fader.StartFadeIn(FadeDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.GetComponent<NetworkObject>().HasStateAuthority) return;
        // 아웃라인 표시 로직
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.GetComponent<NetworkObject>().HasStateAuthority) return;
        // 아웃라인 비표시 로직
    }
}
