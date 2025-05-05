using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;
    public int maxEnemies = 5;

    private int spawnedEnemies = 0;
    private bool isSpawning = false;

    void Update()
    {
        if (!isSpawning && spawnedEnemies < maxEnemies)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        isSpawning = true;

        foreach (Transform point in spawnPoints)
        {
            if (spawnedEnemies >= maxEnemies)
                break;

            Instantiate(enemyPrefab, point.position, Quaternion.identity);
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
