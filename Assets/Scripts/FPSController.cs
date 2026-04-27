using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private Transform cameraTransform;

    [Header("Audio")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private float stepInterval = 5f;

    [Header("Head Bob")]
    [SerializeField] private float bobFrequency = 10f;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float sprintBobMultiplier = 1.5f;

    private CharacterController _controller;
    private AudioSource _audioSource;
    private Vector3 _velocity;
    private float _xRotation;
    private float _stepCycle;
    private float _nextStep;
    private bool _wasGrounded;
    private float _bobTimer;
    private Vector3 _cameraStartLocalPos;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        BuildInputActions();
    }

    private void BuildInputActions()
    {
        var map = new InputActionMap("Player");

        _moveAction = map.AddAction("Move", InputActionType.Value);
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up",    "<Keyboard>/w")
            .With("Down",  "<Keyboard>/s")
            .With("Left",  "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        _lookAction = map.AddAction("Look", InputActionType.Value);
        _lookAction.AddBinding("<Mouse>/delta");

        _jumpAction = map.AddAction("Jump", InputActionType.Button);
        _jumpAction.AddBinding("<Keyboard>/space");

        _sprintAction = map.AddAction("Sprint", InputActionType.Button);
        _sprintAction.AddBinding("<Keyboard>/leftShift");

        map.Enable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _nextStep = stepInterval / 2f;
        _cameraStartLocalPos = cameraTransform.localPosition;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandleHeadBob();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            Application.Quit();
    }

    private void HandleLook()
    {
        Vector2 look = _lookAction.ReadValue<Vector2>();

        _xRotation -= look.y * mouseSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x * mouseSensitivity);
    }

    private void HandleMovement()
    {
        bool grounded = _controller.isGrounded;

        if (!_wasGrounded && grounded)
            PlaySound(landSound);

        if (grounded && _velocity.y < 0f)
            _velocity.y = -2f;

        Vector2 move = _moveAction.ReadValue<Vector2>();
        float speed = _sprintAction.IsPressed() ? sprintSpeed : walkSpeed;

        Vector3 moveDir = transform.right * move.x + transform.forward * move.y;
        _controller.Move(moveDir * speed * Time.deltaTime);

        if (grounded && move != Vector2.zero)
            ProgressStepCycle(speed);

        if (_jumpAction.WasPressedThisFrame() && grounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            PlaySound(jumpSound);
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
        _wasGrounded = grounded;
    }

    private void HandleHeadBob()
    {
        bool grounded = _controller.isGrounded;
        Vector2 move = _moveAction.ReadValue<Vector2>();
        bool moving = grounded && move != Vector2.zero;

        if (moving)
        {
            float multiplier = _sprintAction.IsPressed() ? sprintBobMultiplier : 1f;
            _bobTimer += Time.deltaTime * bobFrequency * multiplier;
            float bobOffset = Mathf.Sin(_bobTimer) * bobAmplitude;
            cameraTransform.localPosition = _cameraStartLocalPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            _bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition, _cameraStartLocalPos, Time.deltaTime * bobFrequency);
        }
    }

    private void ProgressStepCycle(float speed)
    {
        _stepCycle += (_controller.velocity.magnitude + speed) * Time.deltaTime;

        if (_stepCycle < _nextStep)
            return;

        _nextStep = _stepCycle + stepInterval;
        PlayFootstep();
    }

    private void PlayFootstep()
    {
        if (footstepSounds.Length == 0) return;

        int n = Random.Range(1, footstepSounds.Length);
        _audioSource.clip = footstepSounds[n];
        _audioSource.PlayOneShot(_audioSource.clip);
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = _audioSource.clip;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
