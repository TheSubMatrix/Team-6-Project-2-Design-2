using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    GameObject player;
    public RunManager Instance{get; private set;}
    //Currency
    public uint coins = 0;

    //Extra Health
    public uint ExtraHealth; 

    //Life Counting
    public uint maxPlayerLives = 0;
    uint playerLives;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerLives = maxPlayerLives;
            UpdatePlayerReference();
        }
        else
        {
            Destroy(this);
        }
    }
    public void OnPlayerDeath()
    {
        if(playerLives > 0)
        {
            playerLives--;
        }
        else
        {
            SceneTransitionManager.Instance?.TranasitionScene("Starting Scene");
            playerLives = maxPlayerLives;
            player = GameObject.FindGameObjectWithTag("Player");
            UpdatePlayerReference();
            ApplyPermenantUpgrades();
        }
    }
    void ApplyPermenantUpgrades()
    {
        Health playerHealth = player?.GetComponent<Health>();
        if(playerHealth != null)
        {
            playerHealth.MaxHealth = playerHealth.MaxHealth + ExtraHealth;
        }
    }
    void UpdatePlayerReference()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player?.GetComponent<Health>()?.OnDeath.AddListener(OnPlayerDeath); 
        Debug.Log("Player setup complete");
    }
}
