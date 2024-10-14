using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    public ObjectPool<Projectile> poolReference;
    public virtual void OnProjectilePulled()
    {

    }
    public virtual void OnProjectileReturned()
    {
        
    }
    protected virtual void OnCollisionEnter(Collision other)
    {
        DestroyOrReturn();
    }
    protected virtual void DestroyOrReturn()
    {
        if(gameObject.activeSelf)
        {
            if(poolReference != null)
            {
                poolReference.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
