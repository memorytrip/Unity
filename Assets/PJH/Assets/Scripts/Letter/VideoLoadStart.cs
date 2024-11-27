using Fusion;
using UnityEngine;

public class VideoLoadStart : NetworkBehaviour
{ 
    private VideoProgressBar videoProgressBar;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            videoProgressBar = FindAnyObjectByType<VideoProgressBar>();

            if (videoProgressBar == null)
            {
                Debug.LogError("VideoProgressBar를 찾을 수 없습니다!");
            }
        }
    }
    
    //[Rpc(RpcSources.All,RpcTargets.All)]
    public void RpcVideoLoadButtonClicked()
    {
        Debug.Log("버튼이 눌렸습니다");
        if (videoProgressBar != null)
        {
            videoProgressBar.isLoading = true;
        }
        else
        {
            Debug.LogError("videoProgressBar가 null입니다. 초기화를 확인하세요.");
        }
    }
}
