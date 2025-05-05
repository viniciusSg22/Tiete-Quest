using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject meleeHitbox;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float meleeDuration = 0.2f;

    private bool isAttacking = false;

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
#else
        if (Input.GetKeyDown(KeyCode.J))
        {
            MeleeAttack();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RangedAttack();
        }
#endif
    }

    public void MeleeAttack()
    {
        if (!isAttacking)
            StartCoroutine(DoMelee());
    }

    public void RangedAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * projectileSpeed;
    }

    private System.Collections.IEnumerator DoMelee()
    {
        isAttacking = true;
        meleeHitbox.SetActive(true);
        yield return new WaitForSeconds(meleeDuration);
        meleeHitbox.SetActive(false);
        isAttacking = false;
    }
}
