using UnityEngine;

public class SimpleShowcaseRotation : MonoBehaviour
{
    [field: SerializeField] private float rotateSpeed = 0.4f;
        
    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.RotateAround(transform.position, Vector3.up, 90f * rotateSpeed * Time.fixedDeltaTime);
    }
}