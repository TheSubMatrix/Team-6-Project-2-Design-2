using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform instantiationPoint;
    public ObjectPool<Projectile> projectilePool;
    void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(ProjectileCreate, OnProjectilePulledFromPool, OnProjectileReturnedToPool);
    }
    Projectile ProjectileCreate()
    {
        Projectile projectileToReturn = Instantiate(projectilePrefab).GetComponent<Projectile>();
        if(projectileToReturn != null)
        {
            projectileToReturn.poolReference = projectilePool;
            projectileToReturn.gameObject.SetActive(true);
            return projectileToReturn;
        }
        else
        {
            Debug.LogWarning("Projectile Not found on prefab. Cannot be pooled");
            return null;
        }
    }
    public void OnProjectilePulledFromPool(Projectile projectilePulled)
    {
        projectilePulled.transform.position = instantiationPoint.position;
        projectilePulled.transform.rotation = instantiationPoint.rotation;
        projectilePulled.gameObject.SetActive(true);
        projectilePulled.OnProjectilePulled();
    }
    public void OnProjectileReturnedToPool(Projectile projectileReturned)
    {
        projectileReturned.gameObject.SetActive(false);
        projectileReturned.OnProjectileReturned();
    }
    public void SpawnProjectile()
    {
        projectilePool.Get();
    }
}
