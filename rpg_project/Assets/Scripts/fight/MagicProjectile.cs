using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 30; 
    private string casterTag = "Untagged";
    private bool hasDealtDamage = false;

    public void Setup(int damageValue, string ownerTag)
    {
        damage = damageValue;
        casterTag = ownerTag;
    }

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(casterTag)) return;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            hasDealtDamage = true;
            damageable.TakeDamage(damage, DamageType.Magical);
            Destroy(gameObject);
        }
        else if (!other.isTrigger) 
        {
            hasDealtDamage = true;
            Destroy(gameObject);
        }
    }
}