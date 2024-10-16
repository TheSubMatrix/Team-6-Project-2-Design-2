using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Collider))]
public class RewardProvider : MonoBehaviour
{
    [SerializeField] Image boonImage;
    [SerializeField] TMP_Text boonTitleText;
    [SerializeField] TMP_Text boonDescriptionText;
    static bool shouldBeActive;
    BoonBase m_boonToAward;
    public BoonBase BoonToAward
    {
        get
        {
            return m_boonToAward;
        }
        set
        {
            m_boonToAward = value;
            UpdateBoonUI();
        }
    }
    void Awake()
    {
        if(BoonToAward != null)
        {
            UpdateBoonUI();
        }
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
            if(BoonToAward != null)
            {
                other.gameObject.GetComponent<PlayerBoonManager>()?.AddBoon(BoonToAward);
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
    void UpdateBoonUI()
    {
        if(boonImage != null)
        {
            boonImage.sprite = BoonToAward.AssociatedImage;
        }
        if(boonTitleText != null)
        {
            boonTitleText.text = BoonToAward.Name;
        }
        if(boonDescriptionText != null)
        {
            boonDescriptionText.text = BoonToAward.Description;
        }
    }
}
