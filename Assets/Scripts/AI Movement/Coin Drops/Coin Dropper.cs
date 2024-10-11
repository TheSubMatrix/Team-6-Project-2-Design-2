using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{
    public GameObject coinPrefab;
    [SerializeField] uint CoinsToDrop = 3;
    const float TIME_BETWEEN_COIN_DROPS = 0.125f;
    void Start() 
    {
        StartCoroutine(SpawnCoinsAsync());
    }

    IEnumerator SpawnCoinsAsync()
    {
        for(int i = 0; i < CoinsToDrop; i++)
        {
            yield return new WaitForSeconds(TIME_BETWEEN_COIN_DROPS);
            Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)), Random.rotation);
        }
        Destroy(gameObject);
    }
}
