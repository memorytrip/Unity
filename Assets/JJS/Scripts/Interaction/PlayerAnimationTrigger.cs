using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAnimationTrigger : NetworkBehaviour
{
    private static readonly int Pickup = Animator.StringToHash("Pickup");
    private Animator _animator;

    public override void Spawned()
    {
        _animator = GetComponent<Animator>();
        ConnectToButton();
    }
    
    private void ConnectToButton()
    {
        if (SceneManager.GetActiveScene().name == "MultiPlayTest")
        {
            var target = FindAnyObjectByType<FindPhotoButton>(FindObjectsInactive.Include);
            var button = target.gameObject.GetComponent<Button>();
            button.onClick.AddListener(TriggerAnimation);
            Debug.Log($"Found? {target.gameObject} {button != null}");
        }
    }

    private void TriggerAnimation()
    {
        _animator.SetTrigger(Pickup);
    }
}
