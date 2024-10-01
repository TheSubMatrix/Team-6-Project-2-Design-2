using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    Image blackoutImage;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(transform.root);
            blackoutImage = GetComponentInChildren<Image>();
            StartCoroutine(FadeAsync(0));
        } 
    }
    public void TranasitionScene(string sceneToTransitionTo, float timeForFadeIn = 0.5f, float delayForFadeIn = 0f, float timeForFadeOut = 0.5f, float delayForFadeOut = 0f)
    {
        StartCoroutine(FadeTransitionAsync(sceneToTransitionTo, timeForFadeIn, delayForFadeIn, timeForFadeIn, timeForFadeOut));
    }
    IEnumerator FadeTransitionAsync(string sceneToTransitionTo, float timeForFadeIn = 0.5f, float delayForFadeIn = 0f, float timeForFadeOut = 0.5f, float delayForFadeOut = 0f)
    {
        yield return FadeAsync(1, timeForFadeIn, delayForFadeIn);
        SceneManager.LoadScene(sceneToTransitionTo);
        yield return FadeAsync(0, timeForFadeOut, delayForFadeOut);
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
