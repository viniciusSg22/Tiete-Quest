using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    void Bounce()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBody"))
        {
            Debug.Log("Se colidiu lateralmente com inimigo");
        }
        else if (other.CompareTag("EnemyTop"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                Bounce();
            }
        }
    }

}
