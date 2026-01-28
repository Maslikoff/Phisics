using UnityEngine;

[RequireComponent(typeof(StepDetector))]
[RequireComponent(typeof(Mover))]
public abstract class StepClimber : MonoBehaviour
{
    private const float MinDistance = 0.1f;

    [SerializeField] protected float ClimbForce = 8f;
    [SerializeField] private float ClimbDuration = 0.2f;
    [SerializeField] private float MinSpeedForClimb = 0.5f;

    protected StepDetector _detector;
    protected IMovement _movement;
    private bool _isClimbing;
    private float _climbTimer;

    protected virtual void Awake()
    {
        _detector = GetComponent<StepDetector>();
        _movement = GetComponent<IMovement>();
    }

    protected virtual void Update()
    {
        if (_isClimbing)
        {
            _climbTimer -= Time.deltaTime;

            if (_climbTimer <= 0) 
                _isClimbing = false;

            return;
        }

        if (_movement?.IsMoving == true && _movement.CurrentSpeed > MinSpeedForClimb && _movement.Direction.magnitude > MinDistance)
            TryClimbStep(_movement.Direction);
    }

    public bool TryClimbStep(Vector3 moveDirection)
    {
        if (_isClimbing || _detector.CheckForStep(moveDirection) == false)
            return false;

        StartClimbing();

        return true;
    }

    private void StartClimbing()
    {
        _isClimbing = true;
        _climbTimer = ClimbDuration;
        ApplyClimbForce(_detector.GetStepAdjustment());
    }

    protected abstract void ApplyClimbForce(Vector3 adjustment);
}