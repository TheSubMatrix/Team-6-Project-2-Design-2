using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAIController : AIController
{
    protected override void Awake()
    {
        aiStates.Add(new RangedAIPatrolState("Patrol"));
        aiStates.Add(new RangedAIChaseState("Chase"));
        currentState = GetState("Patrol");
        base.Awake();
    }
}
