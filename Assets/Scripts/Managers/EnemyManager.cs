using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public GameObject exitPortal;
    public int maxEnemies;
    private int totalEnemiesAlive = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy()
    {
        totalEnemiesAlive++;
    }

    public void UnregisterEnemy()
    {
        totalEnemiesAlive--;

        if (ScoreManager.Instance != null) ScoreManager.Instance.RegisterEnemyKill();

        if (totalEnemiesAlive <= 0 || ScoreManager.Instance.totalEnemiesKilled >= maxEnemies / 4) exitPortal.SetActive(true);
    }
}
