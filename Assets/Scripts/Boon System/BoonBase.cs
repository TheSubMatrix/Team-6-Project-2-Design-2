using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoonBase
{
    public abstract void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation);
}
