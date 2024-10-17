using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinInteractable : MonoBehaviour, IInteractable
{
    public bool canCancelInteraction => true;
    [SerializeField] PlayerChannel playerChannel;
    [SerializeField] CanvasGroup storeGroup;
    public void OnInteractionEnd()
    {
        playerChannel?.ChangePlayerMovementState?.Invoke(true);
        storeGroup.FadeGroup(this, 0);
    }

    public void OnInteractionStart()
    {
        playerChannel?.ChangePlayerMovementState?.Invoke(false);
        Debug.Log("Test");
        storeGroup.FadeGroup(this, 1);
    }

    public void OnInteractionUpdate()
    {
        
    }
}
