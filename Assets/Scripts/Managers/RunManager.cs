using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Events;

public class RunManager : MonoBehaviour
{
    public GameObject Player
    {
        get;
        private set;
    }
    public static RunManager Instance{get; private set;}
    [SerializeField] public PlayerChannel callbackChannel;
    [SerializeField] public UnityEvent<uint> OnCoinCountUpdated = new UnityEvent<uint>();
    //Currency
    uint m_coins = 5;
    public uint Coins
    {
        get
        {
            return m_coins;
        }
        set
        {
            m_coins = value;
            OnCoinCountUpdated.Invoke(value);
        }
    }

    //Extra Health
    public uint ExtraHealth; 

    //Life Counting
    public int maxPlayerLives = 0;

    uint currentSpawnedEnemies = 0;
    #region Scene Managment
    [SerializeReference] uint roomsToCompleteBeforeNextFloor = 3;

    //This is stupid, just serialize my shit unity :-(
    [System.Serializable]
    public class ListWrapperForSerialization<type>
    {
        public List<type> list = new List<type>();
    }
    uint m_currentFloor;
    int m_currentRoomInFloor = -1;
    public int CurrentRoomInFloor
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
    #region Enemy Spawning
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] List<string> levelsToNotSpawnEnemies = new List<string>();
    #endregion
    public List<BoonBase> RewardPool{ get; private set; } = new List<BoonBase>();

    public int PlayerLives = 0;

    [SerializeField] List<LevelProgressionDoor> doors;
    [field:SerializeField] public List<GodData> Gods {get; private set;}= new List<GodData>();
    [SerializeField] public GodData persistantGod;
    void UpdateSceneRewards()
    {
        GameObject[] rewardAwarders = GameObject.FindGameObjectsWithTag("Reward Selection Trigger");
        if(rewardAwarders.Length > 0)
        {
            foreach(GameObject awarder in rewardAwarders)
            {
                RewardProvider rewardProvider = awarder.GetComponent<RewardProvider>();
                if(rewardProvider != null && RewardPool.Count > 0)
                {
                    rewardProvider.BoonToAward = RewardPool[Random.Range(0, RewardPool.Count)];
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
            PlayerLives = maxPlayerLives;
            RewardPool.Clear();
            RewardPool.AddRange(persistantGod.AssociatedBoons);
            SceneTransitionManager.Instance?.OnSceneTransitionCompleted.AddListener(OnSceneChanged);
            callbackChannel.playerCallback.AddListener(OnPlayerGOCallbackRecieved);
            StartCoroutine(AwaitCallbackReturnAsync());
            selectedSceneListToPickFrom = scenesToPickFrom[0].list;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    public void OnPlayerDeath()
    {
        Debug.Log(PlayerLives);
        if(PlayerLives > 0)
        {
            PlayerLives--;
        }
        else
        {
            SceneTransitionManager.Instance?.TransitionScene("Starting Room");
            PlayerLives = maxPlayerLives;
            m_currentRoomInFloor = -1;
            m_currentFloor = 0;
        }
    }
    void Start()
    {
        OnCoinCountUpdated.Invoke(Coins);
    }
    void OnSceneChanged()
    {
        StartCoroutine(OnSceneChanegdAsync());
    }
    void ApplyPermenantUpgrades()
    {
        Health playerHealth = Player?.GetComponent<Health>();
        if(playerHealth != null)
        {
            playerHealth.MaxHealth = playerHealth.MaxHealth + ExtraHealth;
        }
    }
    void UpdateOnDeathListener()
    {
        Player.GetComponent<Health>().OnDeath.AddListener(OnPlayerDeath);
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
        Player = playerGO;
    }

    public void SetupBoonSelection(GodData godData)
    {
        RewardPool.Clear();
        RewardPool.AddRange(persistantGod.AssociatedBoons);
        RewardPool.AddRange(godData.AssociatedBoons);
    }
    public void UpdateDoorReferences()
    {
        doors =  FindObjectsOfType(typeof(LevelProgressionDoor)).Cast<LevelProgressionDoor>().ToList();
    }
    public void HandleEnemyDeath()
    {
        currentSpawnedEnemies--;
        if(currentSpawnedEnemies <= 0)
        {
            foreach(LevelProgressionDoor door in doors)
            {
                door.OpenDoor();
            }
        }
    }
    IEnumerator AwaitCallbackReturnAsync()
    {
        callbackChannel.signalPlayerCallback?.Invoke();
        yield return new WaitUntil(() => Player != null);
        UpdateOnDeathListener();
    }
    /// <summary>
    /// This is needed because the stupid scene doesnt fully load until the next fixed update. Why?
    /// </summary>
    /// <returns></returns>
    IEnumerator OnSceneChanegdAsync()
    {
        yield return new WaitForFixedUpdate();
        GameObject oldPlayer = Player;
        StartCoroutine(AwaitCallbackReturnAsync());
        if(oldPlayer != Player)
        {
            ApplyPermenantUpgrades();
            OnCoinCountUpdated.Invoke(Coins);
        }
        UpdateSceneRewards();
        if(!levelsToNotSpawnEnemies.Contains(SceneManager.GetActiveScene().name) )
        {
            if(enemyPrefabs.Count > 0)
            {
                currentSpawnedEnemies = 0;
                for(int i = 0; i < roomsToCompleteBeforeNextFloor * (CurrentFloor) + (CurrentRoomInFloor + 1); i++)
                {
                    GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], StaticMethods.GetRandomLocationOnNavmesh(), Quaternion.identity);
                    enemy.GetComponent<Health>().OnDeath.AddListener(HandleEnemyDeath);
                    currentSpawnedEnemies++;
                }
            }else
            {
                Debug.LogWarning("No enemy prefabs in run manager");
            }
        }
        UpdateDoorReferences();
    }
}