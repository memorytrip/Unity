using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectPlayerToTree : NetworkBehaviour
{
    private SkinnedMeshRenderer[] _characterMeshes;
    private const string Yggdrasil = "Yggdrasil";
    
    public override void Spawned()
    {
        ConnectCharacterMeshToTree();
    }
    
    private void ConnectCharacterMeshToTree()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(SceneName.Square))
        {
            _characterMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Yggdrasil))
        {
            return;
        }
        //HideCharacter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Yggdrasil))
        {
            return;
        }
        //ShowCharacter();
    }

    private void HideCharacter()
    {
        foreach (var part in _characterMeshes)
        {
            part.enabled = false;
        }
    }

    private void ShowCharacter()
    {
        foreach (var part in _characterMeshes)
        {
            part.enabled = true;
        }
    }
}
