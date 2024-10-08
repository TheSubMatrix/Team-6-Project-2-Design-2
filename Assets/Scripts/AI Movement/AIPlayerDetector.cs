using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class AIPlayerDetector : MonoBehaviour
{
    [SerializeField] UnityEvent<GameObject, bool> playerDetectionUpdated;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDetectionUpdated?.Invoke(other.gameObject, true);
            Debug.Log(other.gameObject);
        }
    }
    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDetectionUpdated?.Invoke(other.gameObject, false);
        }
    }
}
