using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class LevelProgressionDoor : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("Open Door");
    }
}
