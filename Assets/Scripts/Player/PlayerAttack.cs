using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool shouldTryToAttack = true;
    bool isDashing = false;
    [SerializeField] private Animator anim;
    [SerializeField] PlayerChannel playerChannel;

    private void Awake()
    {
        playerChannel.ChangePlayerMovementState.AddListener(OnPlayerMovementStateChanged);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isDashing && shouldTryToAttack)
        {
            if(anim.GetCurrentAnimatorStateInfo(1).IsName("No Extra Motion") && !anim.IsInTransition(1))
            {
                SoundManager.instance?.PlaySound(transform, SoundManager.instance.FindSoundInfoByName("Sword Swoosh"));
                anim.SetTrigger("Sword Hit 1");
            }
            if(anim.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 1") && !anim.IsInTransition(1))
            {
                SoundManager.instance?.PlaySound(transform, SoundManager.instance.FindSoundInfoByName("Sword Swoosh"));
                anim.SetTrigger("Sword Hit 2");
            }
            if(anim.GetCurrentAnimatorStateInfo(1).IsName("Sword Swing 2") && !anim.IsInTransition(1))
            {
                SoundManager.instance?.PlaySound(transform, SoundManager.instance.FindSoundInfoByName("Sword Swoosh"));
                anim.SetTrigger("Sword Hit 3");
            }
        }
    }
    public void OnDashStarted()
    {
        if(!anim.GetAnimatorTransitionInfo(1).IsName("Cancel Swing 1") &&
         !anim.GetAnimatorTransitionInfo(1).IsName("Cancel Swing 2") &&
         !anim.GetAnimatorTransitionInfo(1).IsName("Cancel Swing 3") && 
         !anim.GetCurrentAnimatorStateInfo(1).IsName("No Extra Motion"))
        {
            anim.SetTrigger("Cancel Swing To Roll");
        }
        isDashing = true;
    }
    public void OnDashEnded()
    {
        isDashing = false;
    }
    public void OnPlayerMovementStateChanged(bool newMovementState)
    {
        shouldTryToAttack = newMovementState;
    }
}
