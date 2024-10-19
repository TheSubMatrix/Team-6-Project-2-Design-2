using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProcAdder : MonoBehaviour
{
    StatusEffect fireEffect = Resources.Load("Fire DOT") as StatusEffect;
    void OnCollisionEnter(Collision other)
    {
        StatusManager statusManager = other.gameObject.GetComponent<StatusManager>();
        if(statusManager != null && fireEffect != null)
        {
            statusManager.ApplyStatus(fireEffect);
        }
    }
}
