using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonAdditionTester : MonoBehaviour
{
    [SerializeField] DebugBoon debugBoon;
    private void Start() {
        FindAnyObjectByType<PlayerBoonManager>().AddBoon(debugBoon);
    }
}
