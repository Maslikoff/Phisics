using UnityEngine;

[RequireComponent(typeof(StepDetector))]
[RequireComponent(typeof(BotMover))]
public class BotStepClimber : StepClimber
{
    private BotMover _botMover;
    private Rigidbody _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _botMover = GetComponent<BotMover>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void ApplyClimbForce(Vector3 adjustment)
    {
        if (_rigidbody != null)
            _rigidbody.AddForce(adjustment * ClimbForce, ForceMode.VelocityChange);
    }
}
