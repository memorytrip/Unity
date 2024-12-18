using Common;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    private CharacterController cc;
    //private NetworkMecanimAnimator networkanim;

    [Header("PlayerMove")] 
    public float playerMoveSpeed;
    public Vector3 playerDir;

    [Header("PlayerJump")] 
    public float jumpForce = 10f;
    private float gravity = -9.81f;
    private float velocity;
    private float gravityMultiplier = 3f;

    [Header("PlayerCamera")] 
    private Camera camera;
    
    [Header("Rotation")]
    public float turnSpeed = 1000f;

    [Header("GroundCheck")] 
    public Vector3 boxSize;
    public float maxDistance = 1f;

    private ChatDisplay _chatDisplay;
    private PlayerInput _playerInput;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        cc = GetComponent<CharacterController>();
        cc.enabled = false;
        //networkanim = GetComponentInChildren<NetworkMecanimAnimator>();
        camera = Camera.main;
        boxSize = new Vector3(1f, 1f, 1f);
    }
    
    public override void Spawned()
    {
        cc.enabled = true;
        SettingCamera();
        InputManager.Instance.jumpAction.started += PlayerJump;
        
        _chatDisplay = FindAnyObjectByType<ChatDisplay>();
        _chatDisplay.StartTyping += ToggleMovement;
        _chatDisplay.StopTyping += ToggleMovement;
    }


    private void SettingCamera()
    {
        if (!HasStateAuthority)
            return;
        CinemachineCamera cam = GameObject.Find("CinemachineCamera")?.GetComponent<CinemachineCamera>();
        if (cam != null)
        {
            cam.Target.TrackingTarget = transform;
            var screenshotCam = cam.transform.Find("ScreenshotCamera_Default")?.GetComponent<CinemachineCamera>();
            if (screenshotCam != null)
            {
                screenshotCam.Target.TrackingTarget = transform;
                Debug.Log($"ScreenshotCam: {screenshotCam} Tracking Target: {screenshotCam.Target.TrackingTarget}");
            }
            else
            {
                Debug.Log("Screenshot cam is not found");
            }
            var screenshotSelfieCam = cam.transform.Find("ScreenshotCamera_Player")?.GetComponent<CinemachineCamera>();
            if (screenshotSelfieCam != null)
            {
                var lookAtTransform = transform;
                screenshotSelfieCam.Target.TrackingTarget = transform;
                Debug.Log($"ScreenshotCam: {screenshotSelfieCam} Tracking Target: {screenshotSelfieCam.Target.TrackingTarget}");
            }
            else
            {
                Debug.Log("Screenshot selfie cam is not found");
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;
        PlayerMove();
        PlayerRotation();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        // if (joystick.isInput)
        if (InputManager.Instance.moveAction.ReadValue<Vector2>().magnitude > 0f)
        {
            playerMoveSpeed = 8f;
        }
        else
        {
            playerMoveSpeed = 0f;
        }
        cc.Move(new Vector3(playerDir.x * playerMoveSpeed, velocity, playerDir.z * playerMoveSpeed) * Time.fixedDeltaTime);
    }
    
    private void ApplyGravity()
    {
        if (cc.isGrounded && velocity < 0.0f)
        {
            velocity = -3.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.fixedDeltaTime;
        }
		
        playerDir.y = velocity;
    }

    public void PlayerRotation()
    {
        if (!InputManager.Instance.moveAction.inProgress)
            return;
        playerDir = Quaternion.Euler(0.0f, camera.transform.eulerAngles.y, 0.0f) * playerDir; //new Vector3(joystick.inputDirection.x, 0.0f, joystick.inputDirection.y);
        Quaternion targetRotation = Quaternion.LookRotation(playerDir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    }
    
    public void PlayerMove()
    { 
        // playerDir = new Vector3(joystick.inputDirection.x, 0f, joystick.inputDirection.y);
        Vector2 inputVector = InputManager.Instance.moveAction.ReadValue<Vector2>();
        playerDir = new Vector3(inputVector.x, 0f, inputVector.y);
        playerDir.Normalize();
    }

    public void PlayerJump(InputAction.CallbackContext ctx)
    {
        if (cc.isGrounded)
            velocity = jumpForce;
    }

    // 텍스트창 입력 시 움직임 금지
    private void ToggleMovement(bool isTyping)
    {
        if (isTyping)
        {
            _playerInput.DeactivateInput();
            Debug.Log("얼음");
        }
        else
        {
            _playerInput.ActivateInput();
            Debug.Log($"얼음 -> 땡");
        }
    }
    
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        InputManager.Instance.jumpAction.started -= PlayerJump;
    }
}