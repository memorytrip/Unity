using System.Collections;
using System.Collections.Generic;
using Common;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class PlayerMovement : NetworkBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    [Header("References")]
    private CharacterController cc;
    //private NetworkMecanimAnimator networkanim;

    [Header("PlayerMove")] 
    public float playerMoveSpeed;
    private float _playerMoveSpeed;
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

    [Header("ScreenshotCam LookAt Target")]
    [SerializeField] private Transform screenshotCamPosition;
    
    private readonly WaitForSeconds _wait = new WaitForSeconds(4f);
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    
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

        StartCoroutine(WaitUntilTransitionEnd());
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
                screenshotCam.Target.TrackingTarget = screenshotCamPosition;
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
                screenshotSelfieCam.Target.TrackingTarget = screenshotCamPosition;
                screenshotSelfieCam.Target.LookAtTarget = lookAtTransform;
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
        if (InputManager.Instance.moveAction.ReadValue<Vector2>().magnitude <= 0f)
        {
            animator.SetBool(IsMoving, false);
            _playerMoveSpeed = 0f;
        }
        else
        {
            animator.SetBool(IsMoving, true);
            _playerMoveSpeed = playerMoveSpeed;
        }
        cc.Move(new Vector3(playerDir.x * _playerMoveSpeed, velocity, playerDir.z * _playerMoveSpeed) * Time.fixedDeltaTime);
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
        this.enabled = !isTyping;
        Debug.Log("이건 타이핑중인지 확인하는 것이여: " + isTyping);
        
        if (isTyping)
        {
            _playerInput.DeactivateInput();
            Debug.Log("얼음");
        }
        else
        {
            _playerInput.ActivateInput();
            Debug.Log("얼음 -> 땡");
        }
    }

    private IEnumerator WaitUntilTransitionEnd()
    {
        if (SceneManager.GetActiveScene().name != "Square")
        {
            yield break;
        }
        _playerInput.DeactivateInput();
        yield return _wait;
        _playerInput.ActivateInput();
        Debug.Log("얼음 -> 땡");
    }
    
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        InputManager.Instance.jumpAction.started -= PlayerJump;
        _chatDisplay.StartTyping -= ToggleMovement;
        _chatDisplay.StopTyping -= ToggleMovement;
    }
    
   
}