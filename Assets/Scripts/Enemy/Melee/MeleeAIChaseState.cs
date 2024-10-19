using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MeleeAIChaseState : AIBaseState
{
    public MeleeAIChaseState(string name) : base(name)
    {

    }
    const float DELAY_BEFORE_PLAYER_LOST = 5;
    const float DISTANCE_FOR_PLAYER_ATTACK = 5f;
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
       controller.enemyAnimator.SetBool("Attacking", false);
    }

    public override void OnStateUpdate(AIController controller)
    {

    }

    public override void OnUpdateNavigation(AIController controller)
    {
        controller.navMeshAgent.destination = Player.transform.position;
        if(Vector3.Distance(new Vector3(controller.transform.position.x, 0, controller.transform.position.z), new Vector3(Player.transform.position.x, 0, Player.transform.position.z)) < DISTANCE_FOR_PLAYER_ATTACK)
        {
            controller.enemyAnimator.SetBool("Attacking", true);
        }else
        {
            controller.enemyAnimator.SetBool("Attacking", false);
        }
    }
    IEnumerator AwaitPlayerLossSightAsync(AIController controller)
    {
        yield return new WaitForSeconds(DELAY_BEFORE_PLAYER_LOST);
        controller.ChangeState("Patrol");
        awaitPlayerLossSightDispatched = false;
        awaitPlayerLossSight = null;
        Debug.Log("Player Lost");
    }
}
