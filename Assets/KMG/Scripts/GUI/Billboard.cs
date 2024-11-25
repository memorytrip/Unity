using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;
    private Canvas canvas;
    
    void Start()
    {
        mainCam = Camera.main.transform;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.forward);
    }
}
