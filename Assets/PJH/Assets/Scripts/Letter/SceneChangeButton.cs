using Common;
using Cysharp.Threading.Tasks;
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
            // nextButton.onClick.AddListener(()=>RpcSceneChange(SceneName.Square));
            nextButton.onClick.AddListener(ReturnToSquare);
        }
    }

    void ReturnToSquare()
    {
        SceneManager.Instance.MoveRoom(SceneManager.SquareScene).Forget();
    }
    
    // void RpcSceneChange(string scenename)
    // {
    //     SceneManager.Instance.MoveScene(scenename);
    // }
}
