using System.Collections;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    public string[] enemyPrefabNames;
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;
    public int maxEnemies = 5;

    private int spawnedEnemies = 0;
    private bool isSpawning = false;

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!isSpawning && spawnedEnemies < maxEnemies) StartCoroutine(SpawnEnemy());
    }

    void Start()
    {
        if (EnemyManager.Instance != null) EnemyManager.Instance.maxEnemies = maxEnemies;
    }

    IEnumerator SpawnEnemy()
    {
        isSpawning = true;

        foreach (Transform point in spawnPoints)
        {
            if (spawnedEnemies >= maxEnemies) break;

            string randomEnemyName = enemyPrefabNames[Random.Range(0, enemyPrefabNames.Length)];

            PhotonNetwork.Instantiate("Enemies/" + randomEnemyName, point.position, Quaternion.identity);
            EnemyManager.Instance.RegisterEnemy();

            spawnedEnemies++;
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    public void DecrementEnemyCount()
    {
        spawnedEnemies = Mathf.Max(0, spawnedEnemies - 1);
    }
}
