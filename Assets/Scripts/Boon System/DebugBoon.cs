using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoon : BoonBase
{
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnDashStarted)
        {
            Debug.Log("Boon Activated");
        }
    }
}
