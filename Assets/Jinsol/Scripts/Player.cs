using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.WakeUp();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Collision!!!! {other.gameObject}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger!!!! {other.gameObject}");
    }
}
