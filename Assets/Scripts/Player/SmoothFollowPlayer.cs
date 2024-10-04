using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowPlayer : MonoBehaviour
{
    [SerializeField] Vector3 positionOffset;
    [SerializeField] public Transform pivotToFollow;
    [SerializeField] float followSpeed = .125f;

    Vector3 velocity = new Vector3(0,0,0);
    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void FixedUpdate()
    {
        if(pivotToFollow != null)
        {
            Vector3 desiredPosition = pivotToFollow.position + positionOffset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSpeed);
            transform.position = smoothedPosition;
        }
    }
}
