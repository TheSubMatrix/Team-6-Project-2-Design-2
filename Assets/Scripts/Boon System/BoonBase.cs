using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonBase
{
    public enum BoonApplicationType
    {
        OnDash,
        OnDamaged,
        OnAttack,
        OneShot
    }

    public BoonApplicationType boonApplicationType;
    public virtual void OnBoonActivated()
    {

    }
}
