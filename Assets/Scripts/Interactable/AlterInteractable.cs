using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] CanvasGroup selectionGroup;
    [SerializeField] PlayerChannel playerChannel;
    public bool canCancelInteraction => true;

    public void OnInteractionEnd()
    {
        playerChannel?.ChangePlayerMovementState?.Invoke(true);
        selectionGroup.FadeGroup(this, 0);
    }

    public void OnInteractionStart()
    {
        playerChannel?.ChangePlayerMovementState?.Invoke(false);
        Debug.Log("Test");
        selectionGroup.FadeGroup(this, 1);
    }

    public void OnInteractionUpdate()
    {

    }
}
