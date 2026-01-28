using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BotMover : Mover
{
    [SerializeField] private float _groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask _groundMask = ~0;

    private Rigidbody _rigidbody;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        UseFixedUpdate = true; 

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.drag = 0.5f;
        _rigidbody.angularDrag = 5f;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    protected override void Update()
    {
        base.Update();

        UpdateGroundStatus();
    }

    private void UpdateGroundStatus()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        _isGrounded = Physics.Raycast(rayStart, Vector3.down, out hit, _groundCheckDistance, _groundMask);

        Debug.DrawRay(rayStart, Vector3.down * _groundCheckDistance, _isGrounded ? Color.green : Color.red);
    }

    protected override void ApplyMovement(Vector3 velocity, float deltaTime)
    {
        if (_rigidbody == null) 
            return;

        Vector3 horizontalVelocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
        Vector3 targetPosition = _rigidbody.position + horizontalVelocity * deltaTime;

        _rigidbody.MovePosition(targetPosition);
    }
}