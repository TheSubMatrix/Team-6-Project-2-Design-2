using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour, IStatusEffectable
{
    public List<StatusEffect> StatusEffects { get; private set; }
    public void ApplyStatus(StatusEffect effect)
    {
        StatusEffects.Add(effect);
        StatusEffects[StatusEffects.Count - 1].OnStatusAdded(this);
    }

    public void RemoveStatus(StatusEffect effect)
    {
        StatusEffect effectToRemove = StatusEffects.Find(x => x == effect);
        effectToRemove.OnStatusRemoved(this);
    }
    void Update() 
    {
        foreach(StatusEffect effect in StatusEffects)
        {
            effect.OnStatusTicked(this);
        }
    }
}
