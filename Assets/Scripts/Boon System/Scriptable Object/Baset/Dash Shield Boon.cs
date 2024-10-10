using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Shield Boon", menuName = "Scriptable Objects/Boons/Bastet/Dash Shield Boon")]
public class DashShieldBoon : BoonBase
{
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnDashStarted)
        {
            boonManager.gameObject.GetComponent<Health>().IsInvulnerable = true;
        }

        if(boonActivation == PlayerBoonManager.BoonActivation.OnDashEnded)
        {
            boonManager.gameObject.GetComponent<Health>().IsInvulnerable = false;
        }
    }
}
