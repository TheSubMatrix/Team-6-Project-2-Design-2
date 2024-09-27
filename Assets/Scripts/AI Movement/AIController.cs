using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    AIBaseState currentState;
    AIPatrolState patrolState = new AIPatrolState();
    void Awake()
    {
        currentState = patrolState;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void ChangeState(AIBaseState stateToChangeTo)
    {
        currentState.OnStateExit(this);
        currentState = stateToChangeTo;
        currentState.OnStateEntered(this);
    }
    private void Update()
    {
        currentState.OnStateUpdate(this);
    }
}
