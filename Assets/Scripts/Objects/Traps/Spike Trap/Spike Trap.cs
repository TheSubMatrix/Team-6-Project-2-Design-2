using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] Animator animator;
    List<GameObject> plateActivators = new List<GameObject>();
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IDamagable>() != null)
        {
            plateActivators.Add(other.gameObject);
        }
        if(plateActivators.Count > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Await Compress"))
        {
            animator.SetTrigger("Plate Pressed");
        }
    }
    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        if(plateActivators.Contains(other.gameObject))
        {
            plateActivators.Remove(other.gameObject);
        }
        plateActivators.RemoveAll(activator => activator == null);
        if(plateActivators.Count <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Plate Compress"))
        {
            animator.SetTrigger("Plate Unpressed");
        }
    }
}
