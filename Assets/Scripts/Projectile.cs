using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 10f;

    private Rigidbody _rb;
    private float _currentLifeTime = 0f;

    private bool _isLaunched = false;

    public event Action<Projectile> ReturnToPoolRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isLaunched)
        {
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime >= _lifeTime)
                RequestReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Снаряд попал в: {collision.gameObject.name}");

        Invoke(nameof(ReturnToPoolRequested), 1f);
    }

    public void RequestReturnToPool()
    {
        ReturnToPoolRequested?.Invoke(this);
    }

    public void ResetProjectile()
    {
        _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

        _isLaunched = false;
        _currentLifeTime = 0f;
    }
}