using Fusion;
using UnityEngine;

public class VideoLoadStart : NetworkBehaviour
{
    private VideoProgressBar videoProgressBar; 
    
    public override void Spawned()
    {
        FindLocalVideoProgressBar();
    }

    private void FindLocalVideoProgressBar()
    {
        videoProgressBar = FindAnyObjectByType<VideoProgressBar>();

        if (videoProgressBar == null)
        {
            Debug.LogError("VideoProgressBar를 찾을 수 없습니다! 각 클라이언트에서 오브젝트를 확인하세요.");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcVideoLoadButtonClicked()
    {
        Debug.Log("버튼이 눌렸습니다. 로컬 클라이언트에서 VideoProgressBar 찾기.");
        
        if (videoProgressBar == null)
        {
            FindLocalVideoProgressBar();
        }

        if (videoProgressBar != null)
        {
            videoProgressBar.StartLoading();
        }
        else
        {
            Debug.LogError("VideoProgressBar가 여전히 null입니다. 로컬에서 참조를 확인해주세요.");
        }
    }
}