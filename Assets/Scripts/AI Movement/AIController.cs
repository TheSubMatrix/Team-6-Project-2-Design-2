using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(Collider)), System.Serializable]
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
    protected AIBaseState currentState;
    Rigidbody rb;
    [field:SerializeField] public List<AIBaseState> aiStates {get; private set;} = new List<AIBaseState>();
    public bool StunnedByKnockback {get; private set;} = false;
    GameObject currentPlayerReference;
    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(UpdateNavigationAsync());
    }
    public void ChangeState(string stateName)
    {
        AIBaseState stateToTransitionTo = GetState(stateName);
        if(stateToTransitionTo != null)
        {
            currentState.OnStateExit(this);
            currentState = stateToTransitionTo;
            currentState.UpdatePlayerReference(currentPlayerReference);
            currentState.OnStateEntered(this);
        }
        else
        {
            Debug.LogWarning("State not transitioned to because it could not be found");
        }
    }
    private void Update()
    {
        if(!StunnedByKnockback)
        {
            currentState.OnStateUpdate(this);
        }
    }
    public void OnPlayerCollisionStateChanegd(GameObject player, bool isInRange)
    {
        currentPlayerReference = player;
        currentState.UpdatePlayerReference(player);
        if(isInRange && !Physics.Linecast(transform.position, player.transform.position, visibilityLayerMask))
        {
            currentState.OnPlayerVisibilityUpdated(this, true);
        }
        else
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
        if(!StunnedByKnockback)
        {
            StartCoroutine(OnKnockbackTakenAsync(knockbackDirection));
        }
    }
    IEnumerator UpdateNavigationAsync()
    {
        if(!StunnedByKnockback)
        {
            currentState.OnUpdateNavigation(this);
        }
        yield return new WaitForSeconds(timeBetweenNavigationUpdates);
        StartCoroutine(UpdateNavigationAsync());
    }
    IEnumerator OnKnockbackTakenAsync(Vector3 knockbackDirection)
    {
        yield return null;
        StunnedByKnockback = true;
        currentState.OnKnockbackTaken(this);
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
        StunnedByKnockback = false;
        yield return null;
    }
    protected AIBaseState GetState(string stateName)
    {
        AIBaseState[] validStates = aiStates.Where(state => state.Name == stateName).ToArray();
        if(validStates.Length > 0)
        {
            return validStates[0];
        }
        return null;
    }
}
