using UnityEngine;
using UnityEngine.UI;

public class RaycastShooter : MonoBehaviour
{
    private Vector3 _rayDirection;
    private int _layerAsLayerMask;
    private int _qrLayerMask;
    private float _maxDistance;
    private Collider _hitBoxCollider;
    private RaycastHit _hit;
    private Button _button;

    public static bool HitDetected { get; private set; }
    public static bool FoundQRCode {get; private set;}

    private void Start()
    {
        _layerAsLayerMask = 1 << LayerMask.NameToLayer("Quest");
        _qrLayerMask = 1 << LayerMask.NameToLayer("QR");
        _maxDistance = 300f;
        _hitBoxCollider = GetComponent<Collider>();
        _button = GameObject.Find("Snapshot").GetComponent<Button>();
        _button.onClick.AddListener(NullifyQuest);
    }
    
    private void FixedUpdate()
    {
        SwitchRayDirection();
        DetectQuestObject();
    }
    
    private void SwitchRayDirection()
    {
        switch (ScreenshotManager.ScreenshotCameraType)
        {
            case EScreenshotCameraType.Selfie:
                _rayDirection = -transform.forward;
                break;
            case EScreenshotCameraType.Default:
            default:
                _rayDirection = transform.forward;
                break;
        }
    }
    
    public void DetectQuestObject()
    {
        // Debug.Log(ScreenshotManager.IsScreenshotModeEnabled);

        if (!ScreenshotManager.IsScreenshotModeEnabled)
        {
            return;
        }
        
        HitDetected = Physics.BoxCast(_hitBoxCollider.bounds.center, transform.localScale * 0.5f, _rayDirection,
            out _hit, transform.rotation, _maxDistance, _layerAsLayerMask);

        FoundQRCode = Physics.BoxCast(_hitBoxCollider.bounds.center, transform.localScale * 0.5f, _rayDirection,
            out _hit, transform.rotation, _maxDistance, _qrLayerMask);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (HitDetected)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * _hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * _hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * _maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * _maxDistance, transform.localScale);
        }
    }
    
    public void NullifyQuest()
    {
        if (Physics.BoxCast(_hitBoxCollider.bounds.center, transform.localScale * 0.5f, _rayDirection,
                out _hit, transform.rotation, _maxDistance, _qrLayerMask))
        {
            _hit.collider.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}