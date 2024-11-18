using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCam;
    void Start()
    {
        mainCam = Camera.main.transform;
    }
    
    void FixedUpdate()
    {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
    }
}
