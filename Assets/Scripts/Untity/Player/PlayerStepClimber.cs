using UnityEngine;

[RequireComponent(typeof(StepDetector))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(CharacterController))]
public class PlayerStepClimber : StepClimber
{
    private CharacterController _characterController;
    private PlayerMover _playerMover;

    protected override void Awake()
    {
        base.Awake();
        _playerMover = GetComponent<PlayerMover>();
        _characterController = GetComponent<CharacterController>();
    }

    protected override void ApplyClimbForce(Vector3 adjustment)
    {
        if (_characterController != null)
            _characterController.Move(adjustment);
    }
}