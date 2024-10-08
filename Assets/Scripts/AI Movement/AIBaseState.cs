using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The base state required for an AI State
/// </summary>
[System.Serializable]
public abstract class AIBaseState
{
    public AIBaseState(string stateName)
    {
        Name = stateName;
    }
    public string Name{get; private set;}
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
