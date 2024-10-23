using UnityEngine;
using GUI;
using Unity.Cinemachine;

public class CinemachineInput : MonoBehaviour
{
    private CinemachineInputAxisController ax;

    void Awake()
    {
        ax = GetComponent<CinemachineInputAxisController>();
    }
    void Update()
    {
        if (Utility.IsPointOverGUI())
        {
            ax.enabled = false;
        }
        else
        {
            ax.enabled = true;
            
        }
    }
}