using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GodData
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField]public string Description {get; private set;}
    public List<BoonBase> AssociatedBoons = new List<BoonBase>();
}
