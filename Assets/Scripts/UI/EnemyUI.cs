using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public static EnemyUI Instance;

    public TextMeshProUGUI killCounterText;
    public int maxEnemies;

    private int currentKills = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Start()
    {
        UpdateText();
    }

    public void IncrementKill()
    {
        currentKills++;
        UpdateText();
    }

    private void UpdateText()
    {
        killCounterText.text = $"{currentKills} / {maxEnemies}";
    }
}
