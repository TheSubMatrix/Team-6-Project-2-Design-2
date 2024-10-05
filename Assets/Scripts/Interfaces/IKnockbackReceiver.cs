using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackReceiver 
{
    public void TakeKnockback(Vector3 knockbackDirection);
}
