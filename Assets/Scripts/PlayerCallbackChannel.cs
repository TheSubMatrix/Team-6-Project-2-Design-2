using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "Player Callback Channel", menuName = "Scriptable Objects/Player Callback Scriptable Object")]
public class PlayerCallbackChannel : ScriptableObject
{
    public UnityEvent signalPlayerCallback;
    public UnityEvent<GameObject> playerCallback;
}
