using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class RangedAIChaseState : AIBaseState
{
    public RangedAIChaseState(string name) : base(name)
    {

    }
    const float DELAY_BEFORE_PLAYER_LOST = 5;
    const float IDEAL_RANGE_TO_KEEP = 2;
    const float DISTANCE_FOR_PLAYER_NAVMESH_CHECK = 2;
    const uint TOTAL_CIRCLE_CHECKS_FOR_BEST_POSITION = 16;
    const float DISTANCE_CHECK_ACCEPTABLE_MARGIN = 0.25f;
    bool awaitPlayerLossSightDispatched = false;
    Coroutine awaitPlayerLossSight;

    public override void OnPlayerVisibilityUpdated(AIController controller, bool newVisibilityState)
    {
        if(!newVisibilityState && !awaitPlayerLossSightDispatched && !controller.StunnedByKnockback)
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
            if(awaitPlayerLossSight != null)
            {
                controller.StopCoroutine(awaitPlayerLossSight);
                awaitPlayerLossSight = null;
            }
            awaitPlayerLossSightDispatched = false;
       Debug.Log("Changing State");
    }

    public override void OnStateUpdate(AIController controller)
    {

    }
    public override void OnKnockbackTaken(AIController controller)
    {
            if(awaitPlayerLossSight != null)
            {
                controller.StopCoroutine(awaitPlayerLossSight);
                awaitPlayerLossSight = null;
            }
            awaitPlayerLossSightDispatched = false;
            Debug.Log("Stunned");
    }
    public override void OnUpdateNavigation(AIController controller)
    {
        NavMesh.SamplePosition(Player.transform.position, out NavMeshHit playerHit, DISTANCE_FOR_PLAYER_NAVMESH_CHECK, NavMesh.AllAreas);
        if(playerHit.hit)
        {
            NavMeshPath playerPath = new NavMeshPath();
            float playerDistance = float.MaxValue;
            if(controller.navMeshAgent.CalculatePath(playerHit.position, playerPath))
            {
                for(int i = 1; i < playerPath.corners.Length; i++)
                {
                    playerDistance += Vector3.Distance(playerPath.corners[i - 1], playerPath.corners[i]);
                }
            }
            
            if(playerDistance <= IDEAL_RANGE_TO_KEEP - DISTANCE_CHECK_ACCEPTABLE_MARGIN || playerDistance >= IDEAL_RANGE_TO_KEEP + DISTANCE_CHECK_ACCEPTABLE_MARGIN)
            {
                List<Vector3> placesToCheckForNavPoint = new List<Vector3>();
                for(int i = 0; i < TOTAL_CIRCLE_CHECKS_FOR_BEST_POSITION; i++)
                {
                    float circleAngleToCheck = (i + 1) / (2 * Mathf.PI);
                    Vector3 positionToCheck = new Vector3
                    (
                        IDEAL_RANGE_TO_KEEP * Mathf.Cos(circleAngleToCheck) + playerHit.position.x,
                        playerHit.position.y,
                        IDEAL_RANGE_TO_KEEP * Mathf.Sin(circleAngleToCheck) + playerHit.position.z
                    );
                    NavMesh.SamplePosition(positionToCheck, out NavMeshHit circleHit, DISTANCE_FOR_PLAYER_NAVMESH_CHECK, NavMesh.AllAreas);
                    if(circleHit.hit)
                    {
                        placesToCheckForNavPoint.Add(circleHit.position);
                        Debug.DrawRay(circleHit.position, Vector3.up, Color.red, 1);
                    }
                }
                Vector3 selectedNavLocation = ScoreHeuristic(placesToCheckForNavPoint, controller, playerDistance);
                controller.navMeshAgent.SetDestination(selectedNavLocation);
            }
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
    Vector3 ScoreHeuristic(List<Vector3> possibleNavPoints, AIController controller, float playerDistance)
    {
        float bestScore = float.MinValue;
        Vector3 selectedLocation = controller.transform.position;
        if(possibleNavPoints.Count > 0)
        {
            float currentScore = 0;

            for(int i = 0; i < possibleNavPoints.Count; i++)
            {
                currentScore -= Vector3.Distance(possibleNavPoints[i], controller.transform.position);
                NavMesh.FindClosestEdge(possibleNavPoints[i], out NavMeshHit nearestEdge, NavMesh.AllAreas);
                //Debug.Log(Vector3.Distance(possibleNavPoints[i], nearestEdge.position) * 4);
                currentScore += Vector3.Distance(possibleNavPoints[i], nearestEdge.position) * 4;
                if(currentScore > bestScore)
                {
                    bestScore = currentScore;
                    selectedLocation = possibleNavPoints[i];
                }
            }
        }
        return selectedLocation;
    }
}
