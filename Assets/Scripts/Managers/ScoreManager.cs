using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance;

    public int currentScore = 0;

    public int totalEnemiesKilled = 0;
    public int damageToEnemyPoints = 75;
    public int damageReceivedPenalty = 55;

    public float startTime;
    public float elapsedTime;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient) photonView.RPC("RPC_SetStartTime", RpcTarget.AllBuffered, Time.time);
    }

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

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "EndGame") return;

        elapsedTime = Time.time - startTime;
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

    public void RegisterEnemyKill()
    {
        photonView.RPC("RPC_RegisterEnemyKill", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_AddScore(int value)
    {
        currentScore = Mathf.Max(0, currentScore + value);
    }

    [PunRPC]
    void RPC_RegisterEnemyKill()
    {
        totalEnemiesKilled++;
    }

    [PunRPC]
    void RPC_SetStartTime(float masterStartTime)
    {
        startTime = masterStartTime;
    }
}
