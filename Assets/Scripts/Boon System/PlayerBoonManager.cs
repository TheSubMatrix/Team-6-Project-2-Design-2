using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerBoonManager : MonoBehaviour
{
    public class BoonActivationEvent : UnityEvent<BoonActivation> { }
    public BoonActivationEvent boonActivationEvent = new BoonActivationEvent();
    public enum BoonActivation
    {
        OnDashStarted,
        OnDashEnded,
        OnDamaged,
        OnAttack,
        OnAdded,
        OnDeath
    }
    List<BoonBase> currentBoons = new List<BoonBase>();
    PlayerMovement playerMovement;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnDashStarted.AddListener(OnDashStarted);
        playerMovement.OnDashEnded.AddListener(OnDashEnded);
    }
    public void OnDashStarted()
    {
        boonActivationEvent?.Invoke(BoonActivation.OnDashStarted);
    }
    public void OnDashEnded()
    {
        boonActivationEvent?.Invoke(BoonActivation.OnDashEnded);
    }

    public void AddBoon(BoonBase boonToAdd)
    {
        boonToAdd.OnBoonActivationEvent(BoonActivation.OnAdded);
        boonActivationEvent.AddListener(boonToAdd.OnBoonActivationEvent);
        currentBoons.Add(boonToAdd);
    }
    public void ClearBoons()
    {
        foreach(BoonBase boon in currentBoons)
        {
            boonActivationEvent.RemoveListener(boon.OnBoonActivationEvent);
        }
        currentBoons.Clear();
    }
}
