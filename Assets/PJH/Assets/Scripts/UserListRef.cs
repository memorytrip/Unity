using System;
using TMPro;
using UnityEngine;

public class UserListRef : MonoBehaviour
{
    public static UserListRef Instance;

    public TMP_Text[] list;
    
    private void Awake()
    {
        Instance = this;
    }
}
