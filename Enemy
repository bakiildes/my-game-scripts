using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health = 100f;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
