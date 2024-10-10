using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Max Health Increase", menuName = "Scriptable Objects/Boons/Anubis/Max Health Increase")]
public class MaxHealthIncreseBoon : BoonBase
{
    [SerializeField] uint maxHealthIncreseAmount = 10; 
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            boonManager.gameObject.GetComponent<Health>().MaxHealth += maxHealthIncreseAmount;
        }
    }
}
