using Common;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : NetworkBehaviour
{
    public Button nextButton;

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        ActiveNextButton();
    }

    private void ActiveNextButton()
    {
        /*if (!RequestVideo())
            return;*/

        //if (HasStateAuthority)
        {
            Debug.Log("아 이");
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(()=>RpcSceneChange("VideoLoadTest"));
        }
    }

    private bool RequestVideo()
    {
        //json 오는거 확인하는 로직
        return true;
    }
    
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcSceneChange(string scenename)
    {
        SceneManager.Instance.MoveScene(scenename);
    }
}
