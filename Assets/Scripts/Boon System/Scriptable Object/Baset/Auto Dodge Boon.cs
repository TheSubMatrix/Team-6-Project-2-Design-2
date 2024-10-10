using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Auto Dodge Boon", menuName = "Scriptable Objects/Boons/Bastet/Auto Dodge Boon")]
public class AutoDodgeBoon : BoonBase
{
    uint lastDamageTaken;
    PlayerMovement playerMovementReference;
    [SerializeField]float healPercent = 0.5f;

    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            boonManager.GetComponent<Health>().OnDamage.AddListener(OnDamageTaken);    
            playerMovementReference = boonManager.GetComponent<PlayerMovement>();
        }

        if(boonActivation == PlayerBoonManager.BoonActivation.OnDamaged)
        {
            boonManager.gameObject.GetComponent<Health>().Heal(new HealData((uint)Mathf.FloorToInt((float)lastDamageTaken * healPercent), boonManager.transform.position));
            playerMovementReference?.Dash();
        }
    }
    void OnDamageTaken(DamageData damageData, uint currentHealth, uint newHealth)
    {
        lastDamageTaken = currentHealth - newHealth;
    }

}
