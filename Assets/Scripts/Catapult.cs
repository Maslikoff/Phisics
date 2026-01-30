using UnityEngine;

public class Catapult : MonoBehaviour
{
    private const float MinDistance = 0.01f;

    [Header("References")]
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Spawner _spawner;

    [Header("Catapult Settings")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Rigidbody _rigidbodyWeight;
    [SerializeField] private float _returnSpeed = 5f;
    [SerializeField] private float _throwDelay = 5f;

    private Projectile _currentProjectile;
    private Vector3 _armRestPosition;
    private Quaternion _armRestRotation;
    private float _originalMass;

    private bool _isLoaded = true;
    private bool _isReturning = false;
    private bool _canReturn = false;

    private void Awake()
    {
        _inputHandler.FKeyPressed += OnHandleFKey;
        _inputHandler.FKeyPressed += OnHandleRKey;

        _rigidbodyWeight.isKinematic = true;
        _originalMass = _rigidbodyWeight.mass;

        _armRestPosition = transform.position;
        _armRestRotation = transform.rotation;

        SpawnProjectile();
    }

    private void FixedUpdate()
    {
        if (_isReturning)
            ReturnArm();
    }

    private void OnHandleFKey()
    {
        if (_isLoaded && _isReturning == false)
            ThrowProjectile();
        else if (_canReturn && _isReturning == false)
            StartArmReturn();
    }

    private void OnHandleRKey()
    {
        _rigidbodyWeight.isKinematic = false;
    }

    private void SpawnProjectile()
    {
        _currentProjectile = _spawner.SpawnProjectile(_spawnPoint.position, _spawnPoint.rotation);
        _currentProjectile.transform.SetParent(_spawnPoint);

        _isLoaded = true;
    }

    private void ThrowProjectile()
    {
        _isLoaded = true;
        _currentProjectile.transform.SetParent(null);

        Invoke(nameof(EnableReturn), _throwDelay);
    }

    private void StartArmReturn()
    {
        _isReturning = true;
        _canReturn = false;

        _rigidbodyWeight.isKinematic = true;
        _rigidbodyWeight.mass = 0.1f;
    }

    private void EnableReturn()
    {
        _canReturn = true;
        Debug.Log("Можно вернуть руку катапульты (нажмите F)");

        Invoke(nameof(EnableFiring), _throwDelay);
    }

    private void EnableFiring()
    {
        if (_isReturning == false)
            SpawnProjectile();
        else
            Invoke(nameof(EnableFiring), 1f);
    }

    private void ReturnArm()
    {
        transform.position = Vector3.Lerp(transform.position, _armRestPosition, _returnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _armRestRotation, _returnSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, _armRestPosition);
        float angle = Quaternion.Angle(transform.rotation, _armRestRotation);

        if (distance < MinDistance && angle < 1f)
        {
            _isReturning = false;

            transform.position = _armRestPosition;
            transform.rotation = _armRestRotation;

            _rigidbodyWeight.isKinematic = true;
            _rigidbodyWeight.mass = _originalMass;

            SpawnProjectile();
        }
    }

    private void OnDestroy()
    {
        _inputHandler.FKeyPressed -= OnHandleFKey;
        _inputHandler.FKeyPressed -= OnHandleRKey;
    }
}