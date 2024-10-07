using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState
{
    public void UpdatePlayerReference(GameObject playerReference)
    {
        Player = playerReference;
    }
    public bool isPlayerVisible;
    public GameObject Player;
    public abstract void OnStateEntered(AIController controller);
    public abstract void OnStateUpdate(AIController controller);
    public abstract void OnStateExit(AIController controller);
    public abstract void OnUpdateNavigation(AIController controller);
    public abstract void OnPlayerVisibilityUpdated(AIController controller, bool newVisibilityState);
}
