using System;
using System.Collections;
using Common;
using Common.Network;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;


public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private CharacterController cc;
    //private NetworkMecanimAnimator networkanim;
    public VirtualJoystick joystick;

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
    private LayerMask groundLayer;
    public float maxDistance = 1f;


    private NetworkObject networkObject;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        //networkanim = GetComponentInChildren<NetworkMecanimAnimator>();
        camera = Camera.main;
        groundLayer = LayerMask.GetMask("map");
        boxSize = new Vector3(1f, 1f, 1f);
        networkObject = GetComponent<NetworkObject>();
        
        SettingCamera();
    }

    private void SettingCamera()
    {
        GameManager.Instance.cinemachineCamera.Target.TrackingTarget = transform;
    }

    public void FixedUpdate()
    {
        if (!networkObject.HasStateAuthority)
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
            playerMoveSpeed = 5f;
        }
        else
        {
            playerMoveSpeed = 0f;
        }
        cc.Move(new Vector3(playerDir.x * playerMoveSpeed, velocity, playerDir.z * playerMoveSpeed) * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        if (cc.isGrounded && velocity < 0.0f)
        {
            velocity = -3.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
		
        playerDir.y = velocity;
    }

    public void PlayerRotation()
    {
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

    public void PlayerJump()
    {
        velocity = jumpForce;
    }

    private bool IsGrounded()
    {
        return Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance , groundLayer);
    }
    
}