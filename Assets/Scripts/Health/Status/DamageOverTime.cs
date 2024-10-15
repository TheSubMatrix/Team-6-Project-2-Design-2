using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Over Time Effect", menuName = "Scriptable Objects/Status Effects/Damage Over Time")]
public class DamageOverTime : StatusEffect
{
    [SerializeField] uint damageToDeal = 5;
    [SerializeField] float timeBetweenDamageTicks = 0.5f;
    float elapsedTimeSinceLastDamageTick = 0;
    Health healthToDamage;
    public override void OnStatusAdded(StatusManager statusManager)
    {
        base.OnStatusAdded(statusManager);
        healthToDamage = statusManager.GetComponent<Health>();
    }
    public override void OnStatusTicked(StatusManager statusManager)
    {
        base.OnStatusTicked(statusManager);
        elapsedTimeSinceLastDamageTick += Time.deltaTime;
        if(elapsedTimeSinceLastDamageTick >= timeBetweenDamageTicks)
        {
            healthToDamage?.Damage(new DamageData(damageToDeal, statusManager.transform.position, 0, statusManager.gameObject));
            elapsedTimeSinceLastDamageTick = 0;
        }
    }
    public override void OnStatusRemoved(StatusManager statusManager)
    {
        base.OnStatusRemoved(statusManager);
        elapsedTimeSinceLastDamageTick = 0;
    }
}
