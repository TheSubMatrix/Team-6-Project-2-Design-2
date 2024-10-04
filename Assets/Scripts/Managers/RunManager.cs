using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class RunManager : MonoBehaviour
{
    public GameObject Player
    {
        get;
        private set;
    }
    public static RunManager Instance{get; private set;}
    [SerializeField] public PlayerCallbackChannel callbackChannel;

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
            m_currentRoomInFloor = -1;
            m_currentFloor = 0;
        }
    }
    void OnSceneChanged()
    {
            GameObject oldPlayer = Player;
            callbackChannel.signalPlayerCallback?.Invoke();
            if(oldPlayer != Player)
            {
                UpdateOnDeathListener();
                ApplyPermenantUpgrades();
            }
            UpdateSceneRewards();
            if(!levelsToNotSpawnEnemies.Contains(SceneManager.GetActiveScene().name) )
            {
                if(enemyPrefabs.Count > 0)
                {
                    for(int i = 0; i < roomsToCompleteBeforeNextFloor * (CurrentFloor + 1) + (CurrentRoomInFloor + 1) - 3; i++)
                    {
                        Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], GetRandomLocationOnNavmesh(), Quaternion.identity);
                        Debug.Log(roomsToCompleteBeforeNextFloor * (CurrentFloor + 1) + (CurrentRoomInFloor + 1) - 3);
                    }
                }else
                {
                    Debug.LogWarning("No enemy prefabs in run manager");
                }
            }
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
        Player?.GetComponent<Health>()?.OnDeath.AddListener(OnPlayerDeath); 
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
    Vector3 GetRandomLocationOnNavmesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        int maxIndices = navMeshData.indices.Length - 3;
        int firstVertexSelected = Random.Range(0, maxIndices);
        int secondVertexSelected = Random.Range(0, maxIndices);
        Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];
        if ((int)firstVertexPosition.x == (int)secondVertexPosition.x ||(int)firstVertexPosition.z == (int)secondVertexPosition.z)
        {
            point = GetRandomLocationOnNavmesh();
        }
        else
        {
            point = Vector3.Lerp(firstVertexPosition, secondVertexPosition,Random.Range(0.05f, 0.95f));
        }
        return point;
    }
}