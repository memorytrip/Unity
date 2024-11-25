using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam = null;
    private Canvas canvas;
    
    void Start()
    {
        if (Camera.main != null)
            mainCam = Camera.main.transform;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    
    private void LateUpdate()
    {
        if (mainCam != null)
            transform.LookAt(transform.position + mainCam.forward);
    }
}
