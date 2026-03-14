using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 30;
    void Start() { Destroy(gameObject, 3f); } 
    void Update() { transform.Translate(Vector3.forward * speed * Time.deltaTime); }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage, DamageType.Magical);
        }
        Destroy(gameObject);
    }
}