using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Boon", menuName = "Scriptable Objects/Boons/Anubis/Heal Boon")]
public class HealBoon : BoonBase
{
    [SerializeField] uint healingAmount = 10; 
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            boonManager.gameObject.GetComponent<Health>().Heal(new HealData(healingAmount, boonManager.transform.position));
        }
    }
}
