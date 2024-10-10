using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoonBase: ScriptableObject
{
    public string Name;
    public string Description;
    public abstract void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager);
}
