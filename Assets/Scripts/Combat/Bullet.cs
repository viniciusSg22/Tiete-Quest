using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
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
            DestroyBullet();
            return;
        }

        if ((enemyLayer & layerMask) != 0)
        {
            if (collision.TryGetComponent<Enemy>(out var enemy))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    enemy.TakeDamage(damage);
                }
                else
                {
                    photonView.RPC("RequestDamageEnemy", RpcTarget.MasterClient, enemy.photonView.ViewID, damage);
                }
            }

            DestroyBullet();
        }
    }

    [PunRPC]
    void RequestDamageEnemy(int enemyViewID, int damageAmount)
    {
        PhotonView enemyPV = PhotonView.Find(enemyViewID);
        if (enemyPV != null)
        {
            if (enemyPV.TryGetComponent<Enemy>(out var enemy)) enemy.TakeDamage(damageAmount);
        }
    }

    void DestroyBullet()
    {
        if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        else Destroy(gameObject);
    }
}
