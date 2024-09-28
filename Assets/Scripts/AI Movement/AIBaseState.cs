using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState
{
    public abstract void OnStateEntered(AIController controller);
    public abstract void OnStateUpdate(AIController controller);
    public abstract void OnStateExit(AIController controller);
    public virtual void OnPlayerSeen(AIController controller)
    {

    }
    public virtual void OnPlayerLost(AIController controller)
    {

    }
}
