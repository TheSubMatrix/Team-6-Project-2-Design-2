using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamager : MonoBehaviour
{
    [SerializeField] uint damageToDeal;
    [SerializeField] float knockbackForce;

    public bool hasAlreadyDealtDamage = false;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(!hasAlreadyDealtDamage)
        {
            IDamagable foundDamagable = other.gameObject.GetComponent<IDamagable>();
            IKnockbackReceiver foundKnockbackReceiver = other.gameObject.GetComponent<IKnockbackReceiver>();
            if(foundDamagable != null)
            {
                foundDamagable.Damage(new DamageData(damageToDeal, other.gameObject.transform.position));
            }
            if(foundKnockbackReceiver != null)
            {
                foundKnockbackReceiver.TakeKnockback((other.transform.position - transform.position).normalized * knockbackForce);
            }
        }

    }
}
