using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIBaseState
{
    const float DELAY_BEFORE_PLAYER_LOST = 5;
    bool awaitPlayerLossSightDispatched = false;
    Coroutine awaitPlayerLossSight;

    public override void OnPlayerVisibilityUpdated(AIController controller, bool newVisibilityState)
    {
        if(!newVisibilityState && !awaitPlayerLossSightDispatched)
        {
            awaitPlayerLossSight = controller.StartCoroutine(AwaitPlayerLossSightAsync(controller));
            Debug.Log("Awaiting Player Lost");
        }
        if(newVisibilityState)
        {
            if(awaitPlayerLossSight != null)
            {
                controller.StopCoroutine(awaitPlayerLossSight);
                awaitPlayerLossSight = null;
            }
            awaitPlayerLossSightDispatched = false;
            Debug.Log("Continuing Chase");
        }
    }

    public override void OnStateEntered(AIController controller)
    {
        
    }

    public override void OnStateExit(AIController controller)
    {
       awaitPlayerLossSight = null;
       awaitPlayerLossSightDispatched = false;
    }

    public override void OnStateUpdate(AIController controller)
    {

    }

    public override void OnUpdateNavigation(AIController controller)
    {
        controller.navMeshAgent.destination = Player.transform.position;
    }
    IEnumerator AwaitPlayerLossSightAsync(AIController controller)
    {
        yield return new WaitForSeconds(DELAY_BEFORE_PLAYER_LOST);
        controller.ChangeState(controller.patrolState);
        awaitPlayerLossSightDispatched = false;
        awaitPlayerLossSight = null;
        Debug.Log("Player Lost");
    }
}
