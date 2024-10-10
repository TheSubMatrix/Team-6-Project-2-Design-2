using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoonBase: ScriptableObject
{
    [field: SerializeField] public string Name {get; protected set;}
    [field: SerializeField] public string Description {get; protected set;}
    [field: SerializeField] public Texture2D AssociatedImage {get; protected set;}
    public abstract void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager);
}
