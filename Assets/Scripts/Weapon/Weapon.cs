using UnityEngine;


public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;

    [SerializeField] private WeaponProperties weaponProperties;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float primaryMaxEnergy;
    [SerializeField] private ParticleSystem particleFlshFire;

    private float m_RefireTimer;
    private float currentPrimaryEnergy;
    private bool isEnergyRestored;

    public bool CanFire => m_RefireTimer <= 0 && isEnergyRestored == false;
    public float PrimaryMaxEnergy  => primaryMaxEnergy;
    public float CurrentPrimaryEnergy  => currentPrimaryEnergy;

    private Destructible owner;

     private AudioSource m_AudioSource;

    #region Unity Event 
    private void Start()
    {
        owner = transform.root.GetComponent<Destructible>();
        m_AudioSource = GetComponent<AudioSource>();
        currentPrimaryEnergy = primaryMaxEnergy;
    }

    protected virtual void Update()
    {
        if(m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;

        UpdateEnergy();
    }
    #endregion
    private void UpdateEnergy()
    {
        currentPrimaryEnergy += (float)weaponProperties.EnergyRegenPerSecond * Time.fixedDeltaTime;
        currentPrimaryEnergy = Mathf.Clamp(currentPrimaryEnergy, 0, primaryMaxEnergy);

        if(currentPrimaryEnergy >= weaponProperties.EnergyAmountToStartFire)
            isEnergyRestored = false;
    }

    private bool TryDrawEnergy(int count)
    { 
      if(count == 0)
            return true;
        if (currentPrimaryEnergy >= count)
        { 
            currentPrimaryEnergy -= count;
            return true;
        }

        isEnergyRestored = true;
        return false;
    }

    #region Public API

    public void Fire()
    {
        if (isEnergyRestored == true) return;
        if (CanFire == false) return;
        if(weaponProperties == null) return;

        if (m_RefireTimer > 0) return;
        if (TryDrawEnergy(weaponProperties.EnergyUsage) == false) return;

        //if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;

        //if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;

        Projectile projectile = Instantiate(weaponProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = firePoint.position;
        projectile.transform.forward= firePoint.forward;

        projectile.SetPerentShooter(owner);

        m_RefireTimer = weaponProperties.RateOfFire;

        //SFX
        particleFlshFire.time = 0; 
        particleFlshFire.Play();

        m_AudioSource.clip = weaponProperties.LaunchSFX; 
        m_AudioSource.Play();
            
    }

    public void FirePointLookAt(Vector3 pos)
    {
        Vector3 offset = Random.insideUnitSphere * weaponProperties.SpreadShootRange;

        if (weaponProperties.SpreadShootDistanceFactor != 0)
        {
            offset = offset * Vector3.Distance(firePoint.position, pos) * weaponProperties.SpreadShootDistanceFactor;
        }

        firePoint.LookAt(pos + offset);    
    }

    public void AssignLoadout(WeaponProperties props)
    {
        if (m_Mode != props.Mode) return;

        m_RefireTimer = 0;
        weaponProperties = props;
    }

    public void AddFullEnergy()
    {
        currentPrimaryEnergy = primaryMaxEnergy;
    }

    #endregion
}


