using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class RewardProvider : MonoBehaviour
{
    static bool shouldBeActive;
    public BoonBase boonToAward;
    void Awake()
    {
        if(SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.OnSceneTransitionCompleted.AddListener(OnSceneTransitionCompleted);
        }
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && shouldBeActive)
        {
            if(boonToAward != null)
            {
                other.gameObject.GetComponent<PlayerBoonManager>()?.AddBoon(boonToAward);
            }
            RunManager.Instance.CurrentRoomInFloor++;
            SceneTransitionManager.Instance?.TransitionScene(RunManager.Instance.GetRandomSceneOnFloor(), true);
            shouldBeActive = false;
        }
    }
    void OnSceneTransitionCompleted()
    {
        shouldBeActive = true;
    }
}
