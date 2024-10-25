using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class UserListControll : MonoBehaviour
{
    public TMP_Text[] userListTexts;
    private List<string> userList = new List<string>();
    
    public void AddUser(string nickname)
    {
        if (userList.Count < userListTexts.Length)
        {
            userList.Add(nickname);
            UpdateUserListText();
        }
        else
        {
            Debug.Log("유저 꽉참");
        }
    }
    
    public void RemoveUser(string nickname)
    {
        if (userList.Contains(nickname))
        {
            userList.Remove(nickname);
            UpdateUserListText();
        }
    }
    
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
                userListTexts[i].text = ""; 
            }
        }
    }
}
