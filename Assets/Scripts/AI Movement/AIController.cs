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
    [Header("AI Settings")]
    [SerializeField] float timeBetweenNavigationUpdates = 0.5f;
    [SerializeField] LayerMask visibilityLayerMask;
    AIBaseState currentState;
    Rigidbody rb;    
    public AIChaseState chaseState = new AIChaseState();
    public AIPatrolState patrolState = new AIPatrolState();
    bool stunnedByKnockback = false;
    GameObject currentPlayerReference;
    void Awake()
    {
        currentState = patrolState;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(UpdateNavigationAsync());
    }
    public void ChangeState(AIBaseState stateToChangeTo)
    {
        currentState.OnStateExit(this);
        currentState = stateToChangeTo;
        currentState.UpdatePlayerReference(currentPlayerReference);
        currentState.OnStateEntered(this);
    }
    private void Update()
    {
        currentState.OnStateUpdate(this);
    }
    public void OnPlayerCollisionStateChanegd(GameObject player, bool isInRange)
    {
        currentPlayerReference = player;
        currentState.UpdatePlayerReference(player);
        if(isInRange && !Physics.Linecast(transform.position, player.transform.position, visibilityLayerMask))
        {
            currentState.OnPlayerVisibilityUpdated(this, true);
            Debug.Log("Player Seen");
        }else
        {
            currentState.OnPlayerVisibilityUpdated(this, false);
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
            StartCoroutine(OnKnockbackTakenAsync(knockbackDirection));
        }
    }
    IEnumerator UpdateNavigationAsync()
    {
        if(!stunnedByKnockback)
        {
            currentState.OnUpdateNavigation(this);
        }
        yield return new WaitForSeconds(timeBetweenNavigationUpdates);
        StartCoroutine(UpdateNavigationAsync());
    }
    IEnumerator OnKnockbackTakenAsync(Vector3 knockbackDirection)
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
