using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "Player Channel", menuName = "Scriptable Objects/Channels/Player Channel")]
public class PlayerChannel : ScriptableObject
{
    public UnityEvent signalPlayerCallback;
    public UnityEvent<GameObject> playerCallback;
    public UnityEvent<bool> ChangePlayerMovementState;
}
