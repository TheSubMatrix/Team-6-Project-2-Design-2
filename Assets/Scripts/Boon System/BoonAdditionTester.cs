using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonAdditionTester : MonoBehaviour
{
    DebugBoon debugBoon = new DebugBoon();
    private void Start() {
        FindAnyObjectByType<PlayerBoonManager>().AddBoon(debugBoon);
    }
}
