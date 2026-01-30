using UnityEngine;

public class ProjectilePool : ObjectPool<Projectile>
{
    public Projectile GetProjectileAtPosition(Vector3 position, Quaternion rotation)
    {
        Projectile projectile = GetObject();
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;

        projectile.ReturnToPoolRequested += OnHandleProjectileReturn;

        return projectile;
    }

    private void OnHandleProjectileReturn(Projectile projectile)
    {
        projectile.ReturnToPoolRequested -= OnHandleProjectileReturn;

        projectile.ResetProjectile();
        ReturnObject(projectile);
    }

    public override void ReturnObject(Projectile obj)
    {
        if (obj == null) 
            return;

        obj.ReturnToPoolRequested -= OnHandleProjectileReturn;

        base.ReturnObject(obj);
    }

    private void OnDestroy()
    {
        foreach (var projectile in _activeObjects)
        {
            if (projectile != null)
                projectile.ReturnToPoolRequested -= OnHandleProjectileReturn;
        }
    }
}