using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    AIBaseState currentState;
    [HideInInspector] public GameObject currentPlayer = null;
    
    public AIChaseState chaseState = new AIChaseState();
    public AIPatrolState patrolState = new AIPatrolState();
    void Awake()
    {
        currentState = patrolState;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void ChangeState(AIBaseState stateToChangeTo)
    {
        currentState.OnStateExit(this);
        currentState = stateToChangeTo;
        currentState.OnStateEntered(this);
    }
    private void Update()
    {
        currentState.OnStateUpdate(this);
        if(currentPlayer != null && !Physics.Linecast(transform.position, currentPlayer.transform.position))
        {
            currentState.OnPlayerSeen(this);
        }else
        {
            currentState.OnPlayerLost(this);
        }
    }
    public void OnPlayerCollisionStateChanegd(GameObject player)
    {
        currentPlayer = player;
    }
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
