using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "God Data", menuName = "Scriptable Objects/God Data", order = 0)]
public class GodData : ScriptableObject {
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField] public string Description {get; private set;}
    [field: SerializeField] public Sprite AssociatedImage {get; private set;}
    public List<BoonBase> AssociatedBoons = new List<BoonBase>();
}
