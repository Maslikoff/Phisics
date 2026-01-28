using UnityEngine;

public class StepDetector : MonoBehaviour
{
    private const float MinDistance = 0.01f;
    private const float MinSigment = 0.05f;

    [SerializeField] private float _stepHeight = 0.3f;
    [SerializeField] private float _stepCheckDistance = 0.5f;
    [SerializeField] private float _stepSmooth = 0.1f;
    [SerializeField] private LayerMask _obstacleMask = ~0;

    private RaycastHit _lowerHit;
    private bool _stepDetected;

    public bool CheckForStep(Vector3 moveDirection, float currentHeight = 0f)
    {
        if (moveDirection.magnitude < MinDistance)
        {
            _stepDetected = false;

            return false;
        }

        Vector3 origin = transform.position + Vector3.up * MinSigment;
        Vector3 direction = moveDirection.normalized;

        if (Physics.Raycast(origin, direction, out _lowerHit, _stepCheckDistance, _obstacleMask))
        {
            float obstacleHeight = _lowerHit.point.y - transform.position.y;

            if (obstacleHeight > MinDistance && obstacleHeight <= _stepHeight)
            {
                Vector3 upperOrigin = origin + Vector3.up * (_stepHeight + _stepSmooth);

                if (Physics.Raycast(upperOrigin, direction, _stepCheckDistance, _obstacleMask) == false)
                {
                    _stepDetected = true;

                    return true;
                }
            }
        }

        _stepDetected = false;

        return false;
    }

    public Vector3 GetStepAdjustment()
    {
        if (_stepDetected == false || _lowerHit.collider == null)
            return Vector3.zero;

        float neededHeight = (_lowerHit.point.y - transform.position.y) + _stepSmooth;
        neededHeight = Mathf.Clamp(neededHeight, 0, _stepHeight);

        return Vector3.up * neededHeight;
    }
}