using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public float maxTimeForEffect{get; private set;} = 3;
    public float timeRemaining { get; set; }
    public virtual void OnStatusAdded(StatusManager statusManager)
    {
        timeRemaining = maxTimeForEffect;
    }
    public virtual void OnStatusTicked(StatusManager statusManager)
    {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining <= 0)
        {
            OnStatusRemoved(statusManager);
        }
    }
    public virtual void OnStatusRemoved(StatusManager statusManager)
    {
        statusManager.StatusEffects.Remove(this);
    }
}
