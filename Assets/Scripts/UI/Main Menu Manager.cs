using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnBeginButtonPressed()
    {
        SceneTransitionManager.Instance?.TranasitionScene("Starting Room");
    }
}
