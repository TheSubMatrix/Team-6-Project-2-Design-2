using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This class handles the player movement and dashing
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    Rigidbody m_rigidBody;
    Vector3 m_playerInput;
    Matrix4x4 m_cameraRotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
    [HideInInspector]public bool shouldTryToMove = true;
    bool m_isDashing = false;
    bool m_isInDashCooldown = false;
    [Header("References")]
    [SerializeField]Animator playerModelAnimator;
    [Header("Movement Options")]
    [SerializeField] public float MovementSpeed = 6;
    [SerializeField] float acceleration;
    [SerializeField] float m_turningSpeed = 1440;
    [Header("Dash Options")]
    [SerializeField] float m_dashSpeed = 4;
    [SerializeField] float m_dashTime = .125f;
    [SerializeField] float m_dashCooldown = .5f;
    [Header("Unity Events")]
    [SerializeField] public UnityEvent OnDashStarted = new UnityEvent();
    [SerializeField] public UnityEvent OnDashEnded = new UnityEvent();
    [SerializeField] PlayerChannel playerChannel;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        playerChannel.signalPlayerCallback.AddListener(ReturnPlayerGO);
        playerChannel.ChangePlayerMovementState.AddListener(ChangePlayerMovementState);
    }

    // Update is called once per frame
    void Update()
    {
        //Get player directional input
        m_playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //Get player dash input
        if(Input.GetKeyDown(KeyCode.Space) && !m_isDashing && !m_isInDashCooldown)
        {
            Dash();
        }
        //Handle look direction
        if(m_playerInput != Vector3.zero && !m_isDashing && shouldTryToMove)
        {
            Vector3 skewedInput = m_cameraRotationMatrix.MultiplyPoint3x4(m_playerInput);
            Vector3 desiredLookDirection = (transform.position + skewedInput) - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(desiredLookDirection, Vector2.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, m_turningSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(!m_isDashing && shouldTryToMove)
        {
            Vector3 desiredMovement = new Vector3(transform.forward.x * MovementSpeed * m_playerInput.normalized.magnitude, m_rigidBody.velocity.y, transform.forward.z * MovementSpeed * m_playerInput.normalized.magnitude);
            //Move the rigidbody based on our direction 
            m_rigidBody.velocity = Vector3.MoveTowards(m_rigidBody.velocity, desiredMovement, acceleration);
        }
        float speed = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z).magnitude;
        playerModelAnimator.SetFloat("Speed", speed / 6);
    }
    /// <summary>
    /// Activates a dash coroutine
    /// </summary>
    public void Dash()
    {
        if(shouldTryToMove)
        {
            StartCoroutine(DashAsync());
        }
    }
    /// <summary>
    /// Dashes forward over the given period and distance
    /// </summary>
    /// <returns>Nothing</returns>
    IEnumerator DashAsync()
    {
        //setup variables for upcoming dash
        m_isDashing = true;
        playerModelAnimator.SetTrigger("Dodge");
        OnDashStarted?.Invoke();
        float elapsedTime = 0f;
        //Move the position along the dash a given amount every fixed update
        while(elapsedTime < m_dashTime)
        {
            Vector3 desiredMovement = new Vector3(transform.forward.x * m_dashSpeed, m_rigidBody.velocity.y, transform.forward.z * m_dashSpeed);
            m_rigidBody.velocity = desiredMovement;
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Start dash cooldown
        StartCoroutine(ResetDashAfterCooldown());
        //Change our isDashing variable back so we can move normally
        m_isDashing = false;
        OnDashEnded?.Invoke();
    }
    /// <summary>
    /// Resets the dash after the dash cooldown time
    /// </summary>
    /// <returns>Nothing</returns>
    IEnumerator ResetDashAfterCooldown()
    {
        m_isInDashCooldown = true;
        yield return new WaitForSeconds(m_dashCooldown);
        m_isInDashCooldown = false;
    }
    void ReturnPlayerGO()
    {
        playerChannel.playerCallback?.Invoke(gameObject);
    }
    void ChangePlayerMovementState(bool newPlayerMovementState)
    {
        shouldTryToMove = newPlayerMovementState;
    }
}
