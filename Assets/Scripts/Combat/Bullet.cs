using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out var enemy)) enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}
