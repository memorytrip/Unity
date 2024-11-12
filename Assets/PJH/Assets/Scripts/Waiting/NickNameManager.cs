using System;
using Common.Network;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NickNameManager : MonoBehaviour
{
    public TMP_Text[] userListTexts => UserListRef.Instance.list;

    private async UniTaskVoid Start()
    {
        try
        {
            while (true)
            {
                UpdateUserListText();
                await UniTask.Delay(500);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void UpdateUserListText()
    {
        for (var i = 0; i < Connection.list.Count; ++i)
        {
            userListTexts[i].text = $"{Connection.list[i].playerName} [{(Connection.list[i].hasSceneAuthority?"O":"X")}]";
        }
        
        for (var i = Connection.list.Count; i < userListTexts.Length; ++i)
        {
            userListTexts[i].text = "Non Player";
        }
    }
}
