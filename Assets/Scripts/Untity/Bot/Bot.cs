using UnityEngine;

[RequireComponent(typeof(BotMover))]
[RequireComponent(typeof(BotStepClimber))]
public class Bot : Entity
{
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _stoppingDistance = 2f;
    [SerializeField] private float _speedMultiplier = 0.7f;
    [SerializeField] private Transform _playerTarget;

    private BotMover _mover;
    private Vector3 _currentDirection;
    private bool _targetInRange;

    private enum AIState { Idle, Chase }
    private AIState _currentState = AIState.Idle;

    private void Awake()
    {
        _mover = GetComponent<BotMover>();

        _mover.SetMaxSpeed(BaseSpeed * _speedMultiplier);
    }

    protected override void Update()
    {
        base.Update();

        UpdateTargetDetection();
        UpdateAIState();

        if (_currentDirection.magnitude > 0.1f && _mover != null)
            _mover.RotateTowards(_currentDirection);
    }

    private void FixedUpdate()
    {
        ExecuteAIState();
    }

    private void UpdateTargetDetection()
    {
        if (_playerTarget == null)
        {
            _targetInRange = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, _playerTarget.position);
        _targetInRange = distance <= _detectionRange;
    }

    private void UpdateAIState()
    {
        if (_playerTarget == null)
        {
            _currentState = AIState.Idle;
            return;
        }

        float distance = Vector3.Distance(transform.position, _playerTarget.position);

        if (distance <= _detectionRange && distance > _stoppingDistance)
            _currentState = AIState.Chase;
        else
            _currentState = AIState.Idle;
    }

    private void ExecuteAIState()
    {
        switch (_currentState)
        {
            case AIState.Idle:
                HandleIdle();
                break;

            case AIState.Chase:
                HandleChase();
                break;
        }
    }

    private void HandleIdle()
    {
        _currentDirection = Vector3.zero;
        _mover?.Stop();
    }

    private void HandleChase()
    {
        if (_playerTarget == null)
        {
            HandleIdle();
            return;
        }

        Vector3 toTarget = _playerTarget.position - transform.position;
        float distance = toTarget.magnitude;

        if (distance <= _detectionRange && distance > _stoppingDistance)
        {
            _currentDirection = toTarget.normalized;
            _currentDirection.y = 0;

            _mover?.Move(_currentDirection);
        }
        else
        {
            HandleIdle();
        }
    }
}