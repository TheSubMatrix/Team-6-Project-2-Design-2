using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Reduction Boon", menuName = "Scriptable Objects/Boons/Bastet/Damage Reduction Boon")]
public class DamageReductionBoon : BoonBase
{
    [SerializeField] uint armorToAdd = 5;
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            boonManager.GetComponent<Health>().Armor += armorToAdd;
        }
    }
}
