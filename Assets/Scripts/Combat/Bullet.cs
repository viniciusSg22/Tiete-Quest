using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int layerMask = 1 << collision.gameObject.layer;

        if ((groundLayer & layerMask) != 0)
        {
            Destroy(gameObject);
            return;
        }

        if ((enemyLayer & layerMask) != 0)
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
