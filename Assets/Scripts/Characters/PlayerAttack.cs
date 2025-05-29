using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    [Header("Ranged")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float rangedCooldown = 0.5f;

    private bool canShoot = true;

    public void RangedAttack()
    {
        if (!photonView.IsMine) return;
        if (canShoot) StartCoroutine(FireProjectile());
    }

    private IEnumerator FireProjectile()
    {
        canShoot = false;

        string characterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["CharacterName"];

        switch (characterName)
        {
            case "Advogado":
                characterName = "Lawyer";
                break;
            case "Agrônomo":
                characterName = "Agronomist";
                break;
            case "Bióloga":
                characterName = "Biologist";
                break;
            case "Engenheiro":
                characterName = "Engineer";
                break;
        }

        GameObject projectile = PhotonNetwork.Instantiate($"Characters/{characterName}/Bullet", firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * projectileSpeed;

        AudioManager.Instance.PlaySound(AudioManager.ESoundType.Shoot);

        yield return new WaitForSeconds(rangedCooldown);
        canShoot = true;
    }
}
