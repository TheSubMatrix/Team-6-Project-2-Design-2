using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool canCancelInteraction{ get; }
    GameObject gameObject { get ; } 
    Transform transform { get ; } 
    public void OnInteractionStart();
    public void OnInteractionUpdate();
    public void OnInteractionEnd();
}

