using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerBoonManager : MonoBehaviour
{
    public class BoonActivationEvent : UnityEvent<BoonActivation, PlayerBoonManager> { }
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
        boonActivationEvent?.Invoke(BoonActivation.OnDashStarted, this);
    }
    public void OnDashEnded()
    {
        boonActivationEvent?.Invoke(BoonActivation.OnDashEnded, this);
    }
    public void OnDamageTaken()
    {
        boonActivationEvent?.Invoke(BoonActivation.OnDamaged, this);
    }
    public void AddBoon(BoonBase boonToAdd)
    {
        boonToAdd.OnBoonActivationEvent(BoonActivation.OnAdded, this);
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
