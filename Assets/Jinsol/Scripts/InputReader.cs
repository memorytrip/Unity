namespace Jinsol
{
    using System;
    using System.Collections;
    using Unity.Cinemachine;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.Playables;
    
    /*해 줘야 하는 설정들
    1. InputAction이 Asset > Settings > Input에 생성되어 있을 것.
    2. 이동은 Value > 2D WASD. 둘러보는 건 Value > Delta.
    주의사항: 이름! 정말! 중요! 코드 쓸 때 헷갈리지 않게 조심.*/
    
    public class InputReader : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private Rigidbody _rigidbody;
        private Vector3 _movementComposite;
        private float _targetAngle;
        [field: SerializeField] private float moveSpeed = 5f;
        [field: SerializeField] private float rotationSpeed = 5f;
        private float _sprintMultiplier;
        private const float DefaultSprintMultiplier = 1f;
        [field: SerializeField] private float setSprintMultiplier = 3f;
        private readonly WaitForSeconds _sprintCoolDown = new(7f);
        private readonly WaitForSeconds _jumpDuration = new(2f);
        private bool _isGrounded;
        [field: SerializeField] private float jumpPower = 10f;
        private Vector3 _movementCenter;
        private bool _isDirty;
        private Transform _camera;
        [SerializeField] private CinemachineCamera characterCamera;
        
        [SerializeField] private GameObject footstep;

        private Animator _animator;
        private static readonly int SayHi = Animator.StringToHash("SayHi");
        private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public Vector2 delta;
        [field: SerializeField] private float camMoveSpeed;
        [field: SerializeField] private float camSpeed = 1f;

        private CinemachineBrain _brain; // 1인칭 전환을 위한 시네머신 브레인 참조
        public CinemachineCamera mainCam; // 현재 주도권을 가진 카메라
        public CinemachineCamera defaultCam; // 원래카메라
        public CinemachineCamera firstPersonCam; // 플레이어 시점 카메라
        public bool toggleCam = false; // 껐다켰다 스위치
        private bool _togglePov = false; // 1인칭 시점 스위치
        private const float DefaultBlendTime = 0f; // 1인칭 전환 속도 (기본값)
        private float _scrollY; // 마우스 스크롤 값 저장
        private Quaternion _nextRotation;
        private Vector3 _nextPosition;

        [SerializeField] private PlayableDirector director;

        private Vector3 _directionToCenter;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main.transform;
            _movementCenter = _camera.transform.position;
            _sprintMultiplier = DefaultSprintMultiplier;

            if (Camera.main != null)
            {
                _brain = Camera.main.GetComponent<CinemachineBrain>();
            }
            else
            {
                Debug.LogError($"Scene needs a main camera with Cinemachine Brain!");
            }
        }

        private void Start()
        {
            _playerInput.onActionTriggered += OnActionTriggered;
        }

        private void FixedUpdate()
        {
            if (_playerInput.actions["Move"].IsPressed())
            {
                if (_movementComposite.magnitude < 0.1f)
                {
                    return;
                }
                
                if (Mathf.Approximately(_movementComposite.x, 0f))
                {
                    var cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0f;
                    cameraForward.Normalize();
                    
                    if (_movementComposite.z > 0f)
                    {
                        _rigidbody.transform.forward = cameraForward;
                        _rigidbody.MoveRotation(Quaternion.RotateTowards(Quaternion.identity, _camera.rotation, 90f));
                    }
                    else
                    {
                        _rigidbody.transform.forward = -cameraForward;
                        _rigidbody.MoveRotation(Quaternion.RotateTowards(Quaternion.identity, _camera.rotation, 90f));
                    }
                    
                    _rigidbody.MovePosition(transform.position + Time.fixedDeltaTime * moveSpeed * _sprintMultiplier * _rigidbody.transform.forward);
                }
                else if (Mathf.Approximately(_movementComposite.z, 0f))
                {
                    characterCamera.transform.position = _movementCenter;
                    
                    /*
                    if (_isDirty)
                    {
                        _movementCenter = Camera.main.transform.position;
                        _isDirty = false;
                    }*/

                    _movementCenter = characterCamera.transform.position;
                    var toPlayer = transform.position - _movementCenter;
                    float angleStep = _movementComposite.x * moveSpeed * _sprintMultiplier * Time.fixedDeltaTime;
                    Quaternion rotation = Quaternion.Euler(0f, angleStep, 0f);
                    var newPosition = _movementCenter + rotation * toPlayer;
                    _rigidbody.MovePosition(newPosition);
                    
                    var tangentDirection = Vector3.Cross(Vector3.up, toPlayer).normalized * Mathf.Sign(_movementComposite.x);
                    _rigidbody.MoveRotation(Quaternion.LookRotation(tangentDirection));
                }
            }

            if (!_isGrounded)
            {
                _rigidbody.linearVelocity = new Vector3(0f, Physics.gravity.y * 3f, 0f);
            }
        }

        private void OnZoom(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //_isDirty = false;
            }
            else
            {
                //_isDirty = true;
            }
        }
                
        private void OnMove(InputAction.CallbackContext context)
        {
            var movementX = context.ReadValue<Vector2>().x;
            var movementZ = context.ReadValue<Vector2>().y;

            if (context.started)
            {
                footstep.SetActive(true);
                _animator.SetBool(IsMoving, true);

                if (Mathf.Approximately(movementZ, 0f))
                {
                    /*if (_isDirty)
                    {
                        return;
                    }

                    _movementCenter = Camera.main.transform.position;
                    _isDirty = true;*/
                }
                else if (Mathf.Approximately(movementX, 0f))
                {
                    //_isDirty = false;
                }
            }
            else if (context.performed)
            {
                _movementComposite = new Vector3(movementX, 0f, movementZ);
            }
            else if (context.canceled)
            {
                footstep.SetActive(false);
                _animator.SetBool(IsMoving, false);
            }
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    OnMove(context);
                    break;
                case "Look":
                    break;
                case "Interact":
                    break;
                case "Emote":
                    StartCoroutine(OnEmote(context));
                    break;
                case "Jump":
                    OnJump(context);
                    break;
                case "Previous":
                    break;
                case "Next":
                    break;
                case "Sprint":
                    OnSprint(context);
                    break;
                case "Zoom":
                    OnZoom(context);
                    break;
            }
        }

        private void OnEnable()
        {
            _playerInput.ActivateInput();
            director.played += OnPlayableDirectorPlayed;
            director.stopped += OnPlayableDirectorStopped;
        }

        private void OnDisable()
        {
            _playerInput.DeactivateInput();
            director.played -= OnPlayableDirectorPlayed;
            director.stopped -= OnPlayableDirectorStopped;
        }

        private IEnumerator OnEmote(InputAction.CallbackContext context)
        {
            Debug.Log($"Emote!");
            var a = context.ReadValueAsButton();
            _animator.SetTrigger(SayHi);
            yield return null;
            _animator.ResetTrigger(SayHi);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }
            
            if (!_isGrounded)
            {
                return;
            }
            
            Debug.Log($"Jump!");
            float jumpVelocity = Mathf.Sqrt(jumpPower * -1 * Physics.gravity.y);
            _rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            StartCoroutine(ProcessGroundedBool());
        }

        private IEnumerator ProcessGroundedBool()
        {
            yield return _jumpDuration;
            _isGrounded = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            _isGrounded = other.gameObject.layer == LayerMask.NameToLayer("Default");
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                StartCoroutine(Sprint());
            }
        }

        private IEnumerator Sprint()
        {
            _animator.SetBool(IsSprinting, true);
            _sprintMultiplier = setSprintMultiplier;
            Debug.Log($"Pressed sprint button");
            yield return _sprintCoolDown;
            _animator.SetBool(IsSprinting, false);
            _sprintMultiplier = DefaultSprintMultiplier;
            Debug.Log($"Sprint deactivated!");
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var lookInput = context.ReadValue<Vector2>();
            Debug.Log($"Looking around!");
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            //player.PlayerInteract(); // 이건 좀 뒤에
            Debug.Log("Interacting..." + context);
        }

        public void OnPlayerLookCam(InputAction.CallbackContext context)
        {
            toggleCam = !toggleCam;
            if (toggleCam)
            {
                //playerLookCam.gameObject.SetActive(true);
                //mainCam = playerLookCam;
                //mainCam.Prioritize();
            }
            else
            {
                //playerLookCam.gameObject.SetActive(false);
                mainCam = defaultCam;
                mainCam.Prioritize();
            }
        }

        public void OnFirstPersonToggle(InputAction.CallbackContext context)
        {
            _togglePov = !_togglePov;
            if (_togglePov)
            {
                firstPersonCam.gameObject.SetActive(true);
                _brain.DefaultBlend.Time = 1f;
                mainCam = firstPersonCam;
                mainCam.Prioritize();
            }
            else
            {
                firstPersonCam.gameObject.SetActive(false);
                _brain.DefaultBlend.Time = DefaultBlendTime;
                mainCam = defaultCam;
                mainCam.Prioritize();
            }
        }

        public void OnPlayableDirectorPlayed(PlayableDirector playableDirector)
        {
            //if (director.state == PlayState.Playing) // 타임라인 재생 중에는 인풋 시스템을 비활성화
        }

        public void OnPlayableDirectorStopped(PlayableDirector playableDirector)
        {
            //controllers.Player.Enable();
        }

        public void ForceEnableInput()
        {
            //controllers.Player.Enable();
        }

        public void OnCameraZoom(InputAction.CallbackContext context)
        {
            if (_scrollY < 0 && mainCam.Lens.FieldOfView <= 15)
                mainCam.Lens.FieldOfView = 15;
            else if (_scrollY > 0 && mainCam.Lens.FieldOfView >= 60)
                mainCam.Lens.FieldOfView = 60;
            else
                mainCam.Lens.FieldOfView += _scrollY;
        }

        private void Update()
        {
            /*#region Follow Transform Rotation
            followTransform.transform.rotation *= Quaternion.AngleAxis(delta.x * rotationPower, Vector3.up);
            #endregion

            #region Vertical Rotation
            followTransform.transform.rotation *= Quaternion.AngleAxis(delta.y * rotationPower, Vector3.right);

            var angles = followTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = followTransform.transform.localEulerAngles.x;

            // Clamp the Up/Down Rotation
            if (angle > 180 && angle < 340)
                angles.x = 340;
            else if (angle < 180 && angle > 40)
                angles.x = 40;

            followTransform.transform.localEulerAngles = angles;
            #endregion

            if (Quaternion.Angle(followTransform.transform.rotation, nextRotation) > 1f)
            {
                nextRotation = Quaternion.Slerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
            }

            if (moveComposite.x == 0 && moveComposite.y == 0)
            {
                nextPosition = transform.position;

                if (aimValue == 1)
                {
                    // Set the player rotation based on the look transform
                    transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                    // Reset the y rotaton of the look transform
                    followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
                }

                return;
            }
            float camMoveSpeed = camSpeed * 0.01f;
            Vector3 position = (transform.forward * (moveComposite.y * camMoveSpeed)) + (transform.right * (moveComposite.x * camMoveSpeed));
            nextPosition = transform.position + position;

            // Set the player rotation based on the look transform
            transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
            // Reset the y rotaton of the look transform
            followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);*/
        }
    }
}