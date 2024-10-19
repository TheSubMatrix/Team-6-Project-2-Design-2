using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Fire Damage Boon", menuName = "Scriptable Objects/Boons/Ra/Fire Damage Boon")]
public class FireDamageBoon : BoonBase
{
    [SerializeField] string scriptToAdd;
    MonoBehaviour scriptToChangeStatusOf;
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            scriptToChangeStatusOf = boonManager.transform.Find("Sword").gameObject.AddComponent(Type.GetType(scriptToAdd)) as MonoBehaviour;
            scriptToChangeStatusOf.enabled = false;
        }
        if(boonActivation == PlayerBoonManager.BoonActivation.OnThirdAttackStarted)
        {
            scriptToChangeStatusOf.enabled = true;
        }
        if(boonActivation == PlayerBoonManager.BoonActivation.OnThirdAttackEnded)
        {
            scriptToChangeStatusOf.enabled = false;
        }
    }
}
