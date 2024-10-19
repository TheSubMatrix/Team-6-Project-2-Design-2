using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedProcAdder : MonoBehaviour
{
    StatusEffect fireEffect = Resources.Load("Bleed DOT") as StatusEffect;
    void OnCollisionEnter(Collision other)
    {
        StatusManager statusManager = other.gameObject.GetComponent<StatusManager>();
        if(statusManager != null && fireEffect != null)
        {
            statusManager.ApplyStatus(fireEffect);
        }
    }
}
