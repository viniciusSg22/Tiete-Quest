using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public int damage = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Acertei o inimigo");
        if (collision.TryGetComponent<Enemy>(out var enemy)) enemy.TakeDamage(damage);
    }
}
