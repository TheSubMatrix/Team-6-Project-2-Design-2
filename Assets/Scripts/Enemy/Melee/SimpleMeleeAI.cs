using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMeleeAI : AIController
{
    protected override void Awake()
    {
        aiStates.Add(new MeleeAIChaseState("Chase"));
        aiStates.Add(new MeleeAIPatrolState("Patrol"));
        currentState = GetState("Patrol");
        base.Awake();
    }
    protected override void Update()
    {
        if(enemyAnimator != null)
        {
            enemyAnimator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
    }
}
