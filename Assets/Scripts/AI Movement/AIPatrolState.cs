using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIBaseState
{
    const float DELAY_BEFORE_PLAYER_FOUND = .5f;
    bool awaitPlayerSightDispatched = false;
    bool playerReachedDestination = false;
    Coroutine awaitPlayerSight;

    public override void OnPlayerVisibilityUpdated(AIController controller, bool newVisibilityState)
    {
        if(newVisibilityState && !awaitPlayerSightDispatched)
        {
            awaitPlayerSight = controller.StartCoroutine(AwaitPlayerSightAsync(controller));
            awaitPlayerSightDispatched = true;
            Debug.Log("dispatched Wait routine");
        }
        else if(!isPlayerVisible)
        {
            if(awaitPlayerSight != null)
            {
                controller.StopCoroutine(awaitPlayerSight);
                awaitPlayerSight = null;
            }
            awaitPlayerSightDispatched = false;
        }
    }

    public override void OnStateEntered(AIController controller)
    {
        
    }

    public override void OnStateExit(AIController controller)
    {
        awaitPlayerSight = null;
        awaitPlayerSightDispatched = false;
    }

    public override void OnStateUpdate(AIController controller)
    {

        if
        (
            controller.navMeshAgent.remainingDistance != Mathf.Infinity && 
            controller.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance
        )
        {
            playerReachedDestination = true;
        }
        else
        {
            playerReachedDestination = false;
        }
    }

    public override void OnUpdateNavigation(AIController controller)
    {
        if(playerReachedDestination)
        {
            controller.navMeshAgent.SetDestination(StaticMethods.GetRandomLocationOnNavmesh());
            Debug.Log("Update Destination");
        }  
    }
        IEnumerator AwaitPlayerSightAsync(AIController controller)
    {
        yield return new WaitForSeconds(DELAY_BEFORE_PLAYER_FOUND);
        controller.ChangeState(controller.chaseState);
        awaitPlayerSightDispatched = false;
        Debug.Log("Change State");
    }
}
