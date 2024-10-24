using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class UserListControll : MonoBehaviour
{
    public TMP_Text[] userListTexts; // 4개의 Text를 배열로 선언
    private List<string> userList = new List<string>();

    // 유저가 들어올 때 호출하는 함수
    public void AddUser(string nickname)
    {
        if (userList.Count < userListTexts.Length)
        {
            userList.Add(nickname);
            UpdateUserListText();
        }
        else
        {
            Debug.LogWarning("User list is full.");
        }
    }

    // 유저가 나갈 때 호출하는 함수
    public void RemoveUser(string nickname)
    {
        if (userList.Contains(nickname))
        {
            userList.Remove(nickname);
            UpdateUserListText();
        }
    }

    // Text UI를 업데이트하는 함수
    private void UpdateUserListText()
    {
        for (int i = 0; i < userListTexts.Length; i++)
        {
            if (i < userList.Count)
            {
                userListTexts[i].text = userList[i];
            }
            else
            {
                userListTexts[i].text = ""; // 빈 곳은 비워둠
            }
        }
    }
}
