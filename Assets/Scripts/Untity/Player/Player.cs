using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerStepClimber))]
[RequireComponent(typeof(InputHandler))]
public class Player : Entity
{
    private const float InputDeadzone = 0.1f;

    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private PlayerMover _mover;
    private InputHandler _inputHandler;
    private Camera _mainCamera;
    
    private bool _autoRotate = true;

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
        _inputHandler = GetComponent<InputHandler>();

        _mainCamera = Camera.main; 

        SetupInputListeners();

        if (_mover != null)
            _mover.SetMaxSpeed(_walkSpeed);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetupInputListeners()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveInput += HandleMovement;
            _inputHandler.OnJumpPressed += HandleJump;
        }
    }

    private void HandleMovement(Vector2 input)
    {
        if (_mover == null || _inputHandler == null) 
            return;

        Vector3 moveDirection = GetCameraRelativeMovement(input);

        if (moveDirection.magnitude >= InputDeadzone)
        {
            moveDirection.Normalize();

            _mover.Move(moveDirection);

            if (_autoRotate && input.magnitude > InputDeadzone)
                RotateToMovementDirection(moveDirection);
        }
        else
        {
            _mover.Stop();
        }
    }

    private Vector3 GetCameraRelativeMovement(Vector2 input)
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;

            if (_mainCamera == null) 
                return Vector3.zero;
        }

        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        return (cameraForward * input.y + cameraRight * input.x).normalized;
    }

    private void RotateToMovementDirection(Vector3 moveDirection)
    {
        if (moveDirection.magnitude < InputDeadzone) 
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (_mover is PlayerMover playerMover)
            playerMover.Jump();
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnMoveInput -= HandleMovement;
            _inputHandler.OnJumpPressed -= HandleJump;
        }
    }
}