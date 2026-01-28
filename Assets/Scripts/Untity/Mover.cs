using UnityEngine;

public abstract class Mover : MonoBehaviour, IMovement
{
    private const float MinDistance = 0.1f;

    [Header("Movement Settings")]
    [SerializeField] protected float MaxSpeed = 5f;
    [SerializeField] protected float RotationSpeed = 10f;

    [Header("Physics Settings")]
    [SerializeField] protected float Acceleration = 15f;
    [SerializeField] protected float Deceleration = 20f;
    [SerializeField] protected bool UseFixedUpdate = true;

    protected Vector3 _currentDirection;
    protected Vector3 _targetVelocity;
    protected Vector3 _currentVelocity;
    protected bool _isMovingIntent;
    protected bool _isActuallyMoving;

    public float CurrentSpeed { get; protected set; }
    public float MaxMoveSpeed => MaxSpeed;
    public Vector3 Direction => _currentDirection;
    public bool IsMoving => _isActuallyMoving;

    protected virtual void Update()
    {
        if (UseFixedUpdate == false)
            UpdateMovement(Time.deltaTime);

        CurrentSpeed = _currentVelocity.magnitude;
        _isActuallyMoving = CurrentSpeed > MinDistance;
    }

    protected virtual void FixedUpdate()
    {
        if (UseFixedUpdate)
            UpdateMovement(Time.fixedDeltaTime);
    }

    protected virtual void UpdateMovement(float deltaTime)
    {
        float smoothFactor = _isMovingIntent ? Acceleration : Deceleration;
        _currentVelocity = Vector3.Lerp(_currentVelocity, _targetVelocity, smoothFactor * deltaTime);

        ApplyMovement(_currentVelocity, deltaTime);
    }

    public virtual void Move(Vector3 direction)
    {
        if (direction.magnitude > MinDistance)
        {
            _currentDirection = direction.normalized;
            _targetVelocity = direction * MaxSpeed;
            _isMovingIntent = true;
        }
        else
        {
            Stop();
        }
    }

    public virtual void Stop()
    {
        _currentDirection = Vector3.zero;
        _targetVelocity = Vector3.zero;
        _isMovingIntent = false;
    }

    public virtual void RotateTowards(Vector3 direction)
    {
        if (direction.magnitude > MinDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );
        }
    }

    protected abstract void ApplyMovement(Vector3 velocity, float deltaTime);

    public void SetMaxSpeed(float speed)
    {
        MaxSpeed = speed;
    }

    public void SetAcceleration(float acceleration)
    {
        Acceleration = acceleration;
    }

    public void SetDeceleration(float deceleration)
    {
        Deceleration = deceleration;
    }
}