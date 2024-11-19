using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAnimationTrigger : NetworkBehaviour
{
    private static readonly int Pickup = Animator.StringToHash("Pickup");
    private Animator _animator;
    private PlayerMovement _playerMovement;

    public override void Spawned()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        ConnectToButton();
    }
    
    private void ConnectToButton()
    {
        if (SceneManager.GetActiveScene().name == "FindPhoto")
        {
            var target = FindAnyObjectByType<FindPhotoButton>(FindObjectsInactive.Include);
            var button = target.gameObject.GetComponent<Button>();
            button.onClick.AddListener(TriggerAnimation);
            Debug.Log($"Found? {target.gameObject} {button != null}");
        }
    }

    private void TriggerAnimation()
    {
        if (!HasStateAuthority)
            return;
        StartCoroutine(PlayerStop());
    }

    private IEnumerator PlayerStop()
    {
        _playerMovement.enabled = false;
        _animator.SetTrigger(Pickup);
        yield return new WaitForSeconds(3.5f);
        _playerMovement.enabled = true;
    }
}
