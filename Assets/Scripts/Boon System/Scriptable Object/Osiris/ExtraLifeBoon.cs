using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Extra Life", menuName = "Scriptable Objects/Boons/Osiris/Extra Life")]
public class ExtraLifeBoon : BoonBase
{
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            RunManager.Instance.PlayerLives++;
        }
    }
}
