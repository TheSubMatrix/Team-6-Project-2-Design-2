using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIBaseState
{
    public override void OnStateEntered(AIController controller)
    {
        
    }

    public override void OnStateExit(AIController controller)
    {
        
    }

    public override void OnStateUpdate(AIController controller)
    {
        controller.navMeshAgent.destination = controller.currentPlayer != null? controller.currentPlayer.transform.position : controller.transform.position;
    }
    public override void OnPlayerLost(AIController controller)
    {
        controller.ChangeState(controller.patrolState);
    }
}
