using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Bleed On Third Attack", menuName = "Scriptable Objects/Boons/Osiris/Bleed On Third Attck")]
public class BleedOnThirdAttack : BoonBase
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
