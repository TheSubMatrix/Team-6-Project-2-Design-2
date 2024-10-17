using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    void OnHealthUpButtonPressed()
    {
        if(RunManager.Instance != null && RunManager.Instance.Coins >= 25)
        {
            RunManager.Instance.ExtraHealth += 10;
            RunManager.Instance.Coins -= 25;
        }
    }
        void OnMovementSpeedUpButtonPressed()
    {
        if(RunManager.Instance != null && RunManager.Instance.Coins >= 10)
        {
            RunManager.Instance.ExtraSpeed += 10;
            RunManager.Instance.Coins -= 10;
        }
    }
        void OnExtraLifeButtonPressed()
    {
        if(RunManager.Instance != null && RunManager.Instance.Coins >= 100)
        {
            RunManager.Instance.maxPlayerLives++;
            RunManager.Instance.Coins -= 100;
        }
    }
}
