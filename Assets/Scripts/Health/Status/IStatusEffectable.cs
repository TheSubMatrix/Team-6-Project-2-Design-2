using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffectable
{
    public void ApplyStatus(StatusEffect effect);
    public void RemoveStatus(StatusEffect effect);
}
