using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public GameObject player
    {
        get;
        private set;
    }
    public static RunManager Instance{get; private set;}
    [SerializeField] public PlayerCallbackChannel callbackChannel;
    //Reward handling for next scene
    public BoonBase boonForReward;

    //Currency
    public uint coins = 0;

    //Extra Health
    public uint ExtraHealth; 

    //Life Counting
    public uint maxPlayerLives = 0;
    #region Scene Managment
    [SerializeReference] uint roomsToCompleteBeforeNextFloor = 3;

    //This is stupid, just serialize my shit unity :-(
    [System.Serializable]
    public class ListWrapperForSerialization<type>
    {
        public List<type> list = new List<type>();
    }
    uint m_currentFloor;
    uint m_currentRoomInFloor = 0;
    public uint CurrentRoomInFloor
    {
        get
        {
            return m_currentRoomInFloor;
        }
        set
        {
            if(value >= roomsToCompleteBeforeNextFloor)
            {
                CurrentFloor++;
                m_currentRoomInFloor = 0;
            }
            else
            {
                m_currentRoomInFloor = value;
            }
        }
    }
    uint CurrentFloor 
    {
        get
        {
            return m_currentFloor;
        }
        set
        {
            m_currentFloor = value;
            selectedSceneListToPickFrom = scenesToPickFrom[(int)value].list != null? scenesToPickFrom[(int)value].list : null;
        }
    }
    [SerializeField] List<ListWrapperForSerialization<string>> scenesToPickFrom = new List<ListWrapperForSerialization<string>>();
    List<string> selectedSceneListToPickFrom;
    #endregion
    public List<BoonBase> rewardPool = new List<BoonBase>();
    uint playerLives;

    void UpdateSceneRewards()
    {
        GameObject[] rewardAwarders = GameObject.FindGameObjectsWithTag("Reward Selection Trigger");
        if(rewardAwarders.Length > 0 && rewardAwarders != null)
        {
            foreach(GameObject awarder in rewardAwarders)
            {
                RewardProvider rewardProvider = awarder.GetComponent<RewardProvider>();
                if(rewardProvider != null && rewardPool.Count > 0)
                {
                    rewardProvider.boonToAward = rewardPool[Random.Range(0, rewardPool.Count)];
                }
            }
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerLives = maxPlayerLives;
            UpdateOnDeathListener();
            SceneTransitionManager.Instance?.OnSceneTransitionCompleted.AddListener(OnSceneChanged);
            callbackChannel.signalPlayerCallback?.Invoke();
            selectedSceneListToPickFrom = scenesToPickFrom[0].list;
            callbackChannel.playerCallback.AddListener(OnPlayerGOCallbackRecieved);
        }
        else
        {
            DestroyImmediate(gameObject);
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
            SceneTransitionManager.Instance?.TransitionScene("Starting Scene");
            playerLives = maxPlayerLives;
        }
    }
    void OnSceneChanged()
    {
            GameObject oldPlayer = player;
            callbackChannel.signalPlayerCallback?.Invoke();
            if(oldPlayer != player)
            {
                UpdateOnDeathListener();
                ApplyPermenantUpgrades();
            }
            UpdateSceneRewards();
    }
    void ApplyPermenantUpgrades()
    {
        Health playerHealth = player?.GetComponent<Health>();
        if(playerHealth != null)
        {
            playerHealth.MaxHealth = playerHealth.MaxHealth + ExtraHealth;
        }
    }
    void UpdateOnDeathListener()
    {
        player?.GetComponent<Health>()?.OnDeath.AddListener(OnPlayerDeath); 
    }
    public string GetRandomSceneOnFloor()
    {
        if(selectedSceneListToPickFrom.Count > 0)
        {
            return selectedSceneListToPickFrom[Random.Range(0, selectedSceneListToPickFrom.Count)];
        }
        return "";
    }
    void OnPlayerGOCallbackRecieved(GameObject playerGO)
    {
        player = playerGO;
    }
}