using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class RewardProvider : MonoBehaviour
{
    public BoonBase boonToAward;
    void Awake()
    {

    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(boonToAward != null)
            {
                other.gameObject.GetComponent<PlayerBoonManager>()?.AddBoon(boonToAward);
            }
            RunManager.Instance.CurrentRoomInFloor++;
            SceneTransitionManager.Instance?.TranasitionScene(RunManager.Instance.GetRandomSceneOnFloor());
        }
    }
}
