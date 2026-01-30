using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ProjectilePool _projectilePool;

    public Projectile SpawnProjectile(Vector3 position, Quaternion rotation)
    {
        if (_projectilePool == null)
            return null;

        return _projectilePool.GetProjectileAtPosition(position, rotation);
    }

    public void ReturnProjectile(Projectile projectile)
    {
        if (_projectilePool == null) 
            return;

        projectile.RequestReturnToPool();
    }
}