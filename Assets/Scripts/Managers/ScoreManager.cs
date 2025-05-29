using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance;

    public int currentScore = 0;

    public int damageToEnemyPoints = 75;
    public int damageReceivedPenalty = 55;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddScoreFromEnemyDamage(int damage)
    {
        int points = damage * damageToEnemyPoints;
        photonView.RPC("RPC_AddScore", RpcTarget.AllBuffered, points);
    }

    public void SubtractScoreFromPlayerDamage()
    {
        int penalty = damageReceivedPenalty;
        photonView.RPC("RPC_AddScore", RpcTarget.AllBuffered, -penalty);
    }

    [PunRPC]
    void RPC_AddScore(int value)
    {
        currentScore = Mathf.Max(0, currentScore + value);
    }
}
