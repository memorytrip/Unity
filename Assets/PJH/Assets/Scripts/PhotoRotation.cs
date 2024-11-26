using System;
using System.Collections;
using DG.Tweening;
using UnityEditor.UI;
using UnityEngine;

public class PhotoRotation : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(15f * Time.deltaTime, 15f * Time.deltaTime, 15f * Time.deltaTime);
    }
}
