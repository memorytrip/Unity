using Fusion;
using UnityEngine;

public class VideoLoadStart : MonoBehaviour
{ 
    private VideoProgressBar videoProgressBar;

    public void Awake()
    {
        videoProgressBar = FindAnyObjectByType<VideoProgressBar>();
    }
    
    [Rpc(RpcSources.All,RpcTargets.All)]
    public void RpcVideoLoadButtonClicked()
    {
        Debug.Log("버튼이 눌렸습니다");
        videoProgressBar.isLoading = true;
    }
}
