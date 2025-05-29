using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Transform spawnPoint;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        if (photonView.IsMine)
        {
            GameObject spawnObj = GameObject.FindWithTag("SpawnPoint");
            if (spawnObj != null) spawnPoint = spawnObj.transform;
        }
    }

    public void TakeDamage()
    {
        if (!photonView.IsMine || isInvulnerable) return;

        photonView.RPC("HandleDamage", RpcTarget.All);
    }

    [PunRPC]
    void HandleDamage()
    {
        if (photonView.IsMine && spawnPoint != null)
        {
            ScoreManager.Instance.SubtractScoreFromPlayerDamage();


            transform.position = spawnPoint.position;
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float duration = 3f;
        float timer = 0f;
        bool show = true;

        while (timer < duration)
        {
            spriteRenderer.color = show ? Color.gray : originalColor;
            show = !show;
            timer += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }

        spriteRenderer.color = originalColor;
        isInvulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("EnemyBody")) TakeDamage();
    }
}
