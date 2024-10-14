using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ArrowProjectile : Projectile
{
    Rigidbody rb;
    [SerializeField] float destroyTime = 2f;
    [SerializeField] uint damageToDeal = 10;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }
    public override void OnProjectilePulled()
    {
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
        StartCoroutine(DestroyOrReturnAfterTime());
    }
    IEnumerator DestroyOrReturnAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        DestroyOrReturn();
    }
    protected override void DestroyOrReturn()
    {
        rb.velocity = Vector3.zero;
        base.DestroyOrReturn();
    }
    protected override void OnCollisionEnter(Collision other) 
    {
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if(damagable != null)
        {
            damagable.Damage(new DamageData(damageToDeal, transform.position, 0, gameObject));
        }
        base.OnCollisionEnter(other);
    }
}
