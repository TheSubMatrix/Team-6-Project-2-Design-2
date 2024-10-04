using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnBeginButtonPressed()
    {
        SceneTransitionManager.Instance?.TransitionScene("Starting Room");
    }

    public void OnCreditsButtonPressed()
    {
        SceneTransitionManager.Instance?.TransitionScene("CreditsScene");
    }

    public void OnHelpButtonPressed()
    {
        SceneTransitionManager.Instance?.TransitionScene("HelpScene");
    }

    public void OnBackButtonPressed()
    {
        SceneTransitionManager.Instance?.TransitionScene("TitleScene");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
