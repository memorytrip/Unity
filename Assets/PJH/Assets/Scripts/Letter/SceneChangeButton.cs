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
        {
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(()=>RpcSceneChange(SceneName.Square));
        }
    }
    
    void RpcSceneChange(string scenename)
    {
        SceneManager.Instance.MoveScene(scenename);
    }
}
