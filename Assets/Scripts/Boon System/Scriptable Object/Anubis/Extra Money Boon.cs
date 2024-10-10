using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Extra Money Boon", menuName = "Scriptable Objects/Boons/Anubis/Extra Money Boon")]
public class ExtraMoneyBoon : BoonBase
{
    [SerializeField] uint extraMoneyToGive = 10;
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            RunManager.Instance.Coins += extraMoneyToGive;
        }
    }
}
