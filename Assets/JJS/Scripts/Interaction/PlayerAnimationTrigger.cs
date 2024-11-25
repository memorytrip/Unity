using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAnimationTrigger : NetworkBehaviour
{
    private static readonly int Pickup = Animator.StringToHash("Pickup");
    private NetworkMecanimAnimator _animator;
    private PlayerMovement _playerMovement;

    public override void Spawned()
    {
        _animator = GetComponent<NetworkMecanimAnimator>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        ConnectToButton();
    }
    
    private void ConnectToButton()
    {
        if (SceneManager.GetActiveScene().name == SceneName.FindPhoto)
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
        // _animator.SetTrigger(Pickup);
        RpcAnimSetTrigger(Pickup);
        yield return new WaitForSeconds(3.5f);
        _playerMovement.enabled = true;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcAnimSetTrigger(int triggerId)
    {
        _animator.SetTrigger(triggerId);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcAnimSetBool(int booleanId, bool value)
    {
        _animator.SetBool(booleanId, value);
    }
}
