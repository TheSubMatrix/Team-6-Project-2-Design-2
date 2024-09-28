using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIBaseState
{
    public override void OnStateEntered(AIController controller)
    {
        
    }

    public override void OnStateExit(AIController controller)
    {
        
    }

    public override void OnStateUpdate(AIController controller)
    {
        if(!controller.navMeshAgent.hasPath || controller.navMeshAgent.velocity.sqrMagnitude <= controller.navMeshAgent.stoppingDistance)
        {
            controller.navMeshAgent.SetDestination(RandomNavmeshLocation(controller, 20));
        }   
    }
    Vector3 RandomNavmeshLocation(AIController controller, float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += controller.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) 
        {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
    public override void OnPlayerSeen(AIController controller)
    {
        controller.ChangeState(controller.chaseState);
    }
}
