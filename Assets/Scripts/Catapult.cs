using UnityEngine;

public class Catapult : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private SpringJoint _springJoint;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Rigidbody _counterWeight;

    [Header("Catapult Settings")]
    [SerializeField] private float _throwForce = 1000f;
    [SerializeField] private float _throwDelay = 6f;

    private Projectile _currentProjectile;
    private bool _isLoaded = true;
    private bool _canReturn = false;

    private void Awake()
    {
        if (_inputHandler != null)
        {
            _inputHandler.FKeyPressed += OnHandleFKey;
            _inputHandler.RKeyPressed += OnHandleRKey;
        }

        _springJoint.spring = _throwForce;

        SpawnProjectile();
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.FKeyPressed -= OnHandleFKey;
            _inputHandler.RKeyPressed -= OnHandleRKey;
        }
    }

    private void OnHandleFKey()
    {
        if (_isLoaded)
            ThrowProjectile();
    }

    private void OnHandleRKey()
    {
        ReturnCatapult();
    }

    private void SpawnProjectile()
    {
        if (_spawner == null || _spawnPoint == null) 
            return;

        _currentProjectile = _spawner.SpawnProjectile(_spawnPoint.position, _spawnPoint.rotation);

        if (_currentProjectile != null)
        {
            _currentProjectile.transform.SetParent(_spawnPoint);
            _isLoaded = true;
        }
    }

    private void ThrowProjectile()
    {
        if (_currentProjectile == null) 
            return;

        _isLoaded = false;

        if (_counterWeight != null)
            _counterWeight.isKinematic = false;

        _currentProjectile.transform.SetParent(null);
        _springJoint.spring = 0f;

        Debug.Log("Катапульта выстрелила!");

        Invoke(nameof(AllowReturn), _throwDelay);
    }

    private void AllowReturn()
    {
        _canReturn = true;
        Debug.Log("Нажмите R для возврата катапульты");
    }

    private void ReturnCatapult()
    {
        if (_canReturn == false || _counterWeight == null)
            return;

        _counterWeight.isKinematic = true;
        _springJoint.spring = _throwForce;

        Invoke(nameof(EnableCounterweightPhysics), 0.1f);

        _canReturn = false;

        Invoke(nameof(SpawnProjectile), _throwDelay);
    }

    private void EnableCounterweightPhysics()
    {
        if (_counterWeight != null)
            _counterWeight.isKinematic = false;
    }
}