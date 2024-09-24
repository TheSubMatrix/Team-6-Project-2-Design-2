using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody m_rigidBody;
    Vector3 m_playerInput;
    Matrix4x4 m_cameraRotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
    bool m_isDashing = false;

    [Header("Movement Options")]
    [SerializeField] float m_movementSpeed = 6;
    [SerializeField] float m_turningSpeed = 1440;
    [Header("Dash Options")]
    [SerializeField] float m_dashDistance = 4;
    [SerializeField] float m_dashTime = .125f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get player directional input
        m_playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //Get player dash input
        if(Input.GetKeyDown(KeyCode.Space) && !m_isDashing)
        {
            StartCoroutine(Dash());
        }
        //Handle look direction
        if(m_playerInput != Vector3.zero && !m_isDashing)
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
        if(!m_isDashing)
        {
            //Move the rigidbody based on our direction 
            m_rigidBody.MovePosition(transform.position + (transform.forward * m_playerInput.normalized.magnitude) * m_movementSpeed * Time.fixedDeltaTime);
        }
    }
    IEnumerator Dash()
    {
        m_isDashing = true;
        float elapsedTime = 0f;
        Debug.Log("Start");
        while(elapsedTime < m_dashTime)
        {

            m_rigidBody.MovePosition(transform.position + (transform.forward *  Time.fixedDeltaTime * (m_dashDistance/m_dashTime)));
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("End");
        m_isDashing = false;
    }
}
