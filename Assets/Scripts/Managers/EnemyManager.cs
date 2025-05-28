using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public GameObject exitPortal;
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

        if (totalEnemiesAlive <= 0) exitPortal.SetActive(true);
    }
}
