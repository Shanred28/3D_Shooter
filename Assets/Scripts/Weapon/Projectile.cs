using UnityEngine;


public class Projectile : Entity
{
    [SerializeField] protected float m_Velocity;
    public float VelocityProjectile => m_Velocity;

    [SerializeField] protected float m_LifeTime;

    [SerializeField] protected int m_Damage;

    [SerializeField] protected ImpactEffect m_ImpactEffectMetallPrefab;
    [SerializeField] protected ImpactEffect m_ImpactEffectStonePrefab;

    protected float m_Timer;

    protected virtual void Update()
    {
        float stepLenght = Time.deltaTime * m_Velocity;
        Vector3 step = transform.forward * stepLenght;

        RaycastHit hit; 

        if (Physics.Raycast(transform.position, transform.forward,out hit, stepLenght) == true 
            && hit.collider.isTrigger == false)
        {
            Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
            if (dest != null && dest != m_Perent)
            {
                 dest.ApplyDamage(m_Damage);
            }

            OnProjectileLifeEnd(hit.collider, hit.point, hit.normal);
        }

        m_Timer += Time.deltaTime;
        if (m_Timer > m_LifeTime)
            Destroy(gameObject);

        transform.position += new Vector3(step.x, step.y, step.z);
    }

    protected void OnProjectileLifeEnd(Collider col, Vector3 pos, Vector3 normal)
    {

        if (col.transform.TryGetComponent(out TypeMaterial typeMatrial))
        {

            if (typeMatrial.materiall == materialType.Metall)
            {

                ImpactEffect impact = Instantiate(m_ImpactEffectMetallPrefab, pos, Quaternion.LookRotation(normal));
                impact.transform.SetParent(col.transform);
            }
            else
            {

                ImpactEffect impact = Instantiate(m_ImpactEffectStonePrefab, pos, Quaternion.LookRotation(normal));
                impact.transform.SetParent(col.transform);
            }
        }
        Destroy(gameObject);
    }

    private Destructible m_Perent;

    public void SetPerentShooter(Destructible perent)
    {
        m_Perent = perent;

    }
}

