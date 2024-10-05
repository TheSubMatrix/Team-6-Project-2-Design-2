using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Collider))]
public class AIController : MonoBehaviour, IKnockbackReceiver
{
    [Header("References")]
    public NavMeshAgent navMeshAgent;
    [Header("Knockback Settings")]
    [Range(0.001f, 0.1f)][SerializeField] float stillThreshold = 0.05f;
    [SerializeField] float stunTime = 0.25f;

    AIBaseState currentState;
    [HideInInspector] public GameObject currentPlayer = null;
    Rigidbody rb;    
    public AIChaseState chaseState = new AIChaseState();
    public AIPatrolState patrolState = new AIPatrolState();
    bool stunnedByKnockback = false;

    void Awake()
    {
        currentState = patrolState;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }
    public void ChangeState(AIBaseState stateToChangeTo)
    {
        currentState.OnStateExit(this);
        currentState = stateToChangeTo;
        currentState.OnStateEntered(this);
    }
    private void Update()
    {
        if(!stunnedByKnockback)
        {
            currentState.OnStateUpdate(this);
            if(currentPlayer != null && !Physics.Linecast(transform.position, currentPlayer.transform.position))
            {
                currentState.OnPlayerSeen(this);
            }
            else
            {
                currentState.OnPlayerLost(this);
            }
        }
    }
    public void OnPlayerCollisionStateChanegd(GameObject player)
    {
        if(!stunnedByKnockback)
        {
            currentPlayer = player;
        }
    }
    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void TakeKnockback(Vector3 knockbackDirection)
    {
        if(!stunnedByKnockback)
        {
            StartCoroutine(OnKnockbackTaken(knockbackDirection));
        }
    }
    IEnumerator OnKnockbackTaken(Vector3 knockbackDirection)
    {
        yield return null;
        stunnedByKnockback = true;
        navMeshAgent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(knockbackDirection, ForceMode.Impulse);
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb.velocity.magnitude <= stillThreshold);
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        navMeshAgent.Warp(transform.position);
        navMeshAgent.enabled = true;
        stunnedByKnockback = false;
        yield return null;
    }
}
