using Unity.VisualScripting;
using UnityEngine;

public class RotateLetterPhoto : MonoBehaviour
{
    //public GameObject letterPhoto;
    private float rotateY = 0f;
    public float turnSpeed = 7200f;
    
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateImage(GameObject letterPhoto)
    {
        Debug.Log("버튼 눌림");
        if (letterPhoto.transform.rotation.y <= 90f && letterPhoto.transform.rotation.y >= 270)
        {
            rotateY += 180f;
            Vector3 dir = new Vector3(0f, rotateY, 0f);
            Quaternion targetRotationY = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotationY, turnSpeed * Time.deltaTime);
            letterPhoto.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            letterPhoto.transform.GetChild(0).gameObject.SetActive(false);
            rotateY -= 180f;
        }
    }
}
