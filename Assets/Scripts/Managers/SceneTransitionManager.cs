using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] public UnityEvent OnSceneTransitionCompleted;
    public static SceneTransitionManager Instance { get; private set; }
    Image blackoutImage;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            DestroyImmediate(gameObject);
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(transform.root);
            blackoutImage = GetComponentInChildren<Image>();
            StartCoroutine(FadeAsync(0));
        } 
    }
    public void TransitionScene(string sceneToTransitionTo, bool transitionPlayer = false, float timeForFadeIn = 0.5f, float delayForFadeIn = 0f, float timeForFadeOut = 0.5f, float delayForFadeOut = 0f)
    {
        StartCoroutine(FadeTransitionAsync(sceneToTransitionTo, transitionPlayer, timeForFadeIn, delayForFadeIn, timeForFadeIn, timeForFadeOut));
    }

    IEnumerator FadeTransitionAsync(string sceneToTransitionTo, bool transitionPlayer = false, float timeForFadeIn = 0.5f, float delayForFadeIn = 0f, float timeForFadeOut = 0.5f, float delayForFadeOut = 0f)
{
    if(RunManager.Instance != null && transitionPlayer)
    {
        DontDestroyOnLoad(RunManager.Instance.player.gameObject);
    }
    yield return FadeAsync(1, timeForFadeIn, delayForFadeIn);
    SceneManager.LoadScene(sceneToTransitionTo);
    while(SceneManager.GetSceneByName(sceneToTransitionTo).isLoaded == false)
    {
        yield return null;
    }
    if(RunManager.Instance != null && RunManager.Instance.player != null && transitionPlayer)
    {
        GameObject foundSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn");
        if(foundSpawnPoint != null)
        {
            RunManager.Instance.player.transform.position = foundSpawnPoint.transform.position;
        }
        yield return null;
        yield return new WaitForEndOfFrame();
        SceneManager.MoveGameObjectToScene(RunManager.Instance.player, SceneManager.GetActiveScene());
        SmoothFollowPlayer[] smoothFollowPlayerScripts = FindObjectsOfType<SmoothFollowPlayer>();
        if(smoothFollowPlayerScripts != null && smoothFollowPlayerScripts.Length > 0)
        {
            foreach(SmoothFollowPlayer i in smoothFollowPlayerScripts)
            {
                i.pivotToFollow = RunManager.Instance.player.transform;
            }
        }
    }
    OnSceneTransitionCompleted?.Invoke();
    yield return FadeAsync(0, timeForFadeOut, delayForFadeOut);
    Debug.Log("Complete");
}
    IEnumerator FadeAsync(float desiredAlpha, float timeForFade = 0.5f, float delay = 0f)
    {
        float elapsedTime = 0;
        float startingAlpha = blackoutImage.color.a;
        yield return new WaitForSeconds(delay);
        while (elapsedTime <= timeForFade)
        {
            blackoutImage.color = new Color(blackoutImage.color.r, blackoutImage.color.b, blackoutImage.color.b, Mathf.Lerp(startingAlpha, desiredAlpha, elapsedTime / timeForFade));
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
    }

}
