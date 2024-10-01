using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnBeginButtonPressed()
    {
        SceneTransitionManager.Instance?.TranasitionScene("Starting Room");
    }

    public void OnCreditsButtonPressed()
    {
        SceneTransitionManager.Instance?.TranasitionScene("CreditsScene");
    }

    public void OnHelpButtonPressed()
    {
        SceneTransitionManager.Instance?.TranasitionScene("HelpScene");
    }

    public void OnBackButtonPressed()
    {
        SceneTransitionManager.Instance?.TranasitionScene("TitleScene");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
