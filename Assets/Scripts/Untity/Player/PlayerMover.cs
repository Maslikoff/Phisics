using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : Mover
{
    private const float GroundCheckInterval = 0.1f;
    private const float ForceGravity = -2f;

    [SerializeField] private float _gravity = -30f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundMask = ~0;

    private CharacterController _controller;
    private Vector3 _verticalVelocity;
    private bool _isGrounded;
    private float _groundCheckTimer;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        UseFixedUpdate = false; 
    }

    protected override void Update()
    {
        base.Update();

        UpdateGroundStatus();
        UpdateGravity();
    }

    private void UpdateGroundStatus()
    {
        _groundCheckTimer -= Time.deltaTime;

        if (_groundCheckTimer > 0)
            return;

        _groundCheckTimer = GroundCheckInterval;
        bool wasGrounded = _isGrounded;

        _isGrounded = _controller.isGrounded;

        if (_isGrounded == false)
        {
            RaycastHit hit;
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            _isGrounded = Physics.Raycast(rayStart, Vector3.down, out hit, _groundCheckDistance, _groundMask);
        }

        if (_isGrounded && wasGrounded == false && _verticalVelocity.y < 0)
            _verticalVelocity.y = ForceGravity;
    }

    private void UpdateGravity()
    {
        if (_isGrounded && _verticalVelocity.y < 0)
            _verticalVelocity.y = ForceGravity; 
        else
            _verticalVelocity.y += _gravity * Time.deltaTime;
    }

    protected override void ApplyMovement(Vector3 velocity, float deltaTime)
    {
        if (_controller == null || _controller.enabled == false)
            return;

        Vector3 horizontalMove = new Vector3(velocity.x, 0, velocity.z) * deltaTime;
        Vector3 verticalMove = _verticalVelocity * deltaTime;
        Vector3 totalMove = horizontalMove + verticalMove;

        _controller.Move(totalMove);
    }

    public override void Move(Vector3 direction)
    {
        base.Move(direction);
    }

    public override void Stop()
    {
        base.Stop();
    }

    public override void RotateTowards(Vector3 direction)
    {
        base.RotateTowards(direction);
    }

    public void Jump()
    {
        if (_isGrounded)
            _verticalVelocity.y = Mathf.Sqrt(_jumpHeight * ForceGravity * _gravity);
    }
}