using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionTest : MonoBehaviour
{
    private bool onOff = true;
    public void Interact()
    {
        if (onOff)
        {
            gameObject.transform.position = transform.position + new Vector3(3f, 0f, 0f);
            onOff = false;
        }
        else
        { 
            onOff = true;
        }
        
    }

}
