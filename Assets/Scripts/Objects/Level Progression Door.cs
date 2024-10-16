using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class LevelProgressionDoor : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;
    [SerializeField] CanvasGroup boonInfoPanel;
    public void OpenDoor()
    {
        if(doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open Door");
        }
        boonInfoPanel.FadeGroup(this, 1);
    }
}
