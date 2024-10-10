using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debug Boon", menuName = "Scriptable Objects/Boons/Debug Boon")]
public class DebugBoon : BoonBase
{
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnDashStarted)
        {
            Debug.Log("Boon Activated");
        }
    }
}
