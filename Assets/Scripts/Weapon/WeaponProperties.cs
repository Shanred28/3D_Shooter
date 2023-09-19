using UnityEngine;


public enum WeaponMode
{ 
    Primary,
    PrimaryDouble,
    Secondary
}

[CreateAssetMenu]
public sealed class WeaponProperties : ScriptableObject
{
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;

    [SerializeField] private Projectile m_ProjectilePrefab;
    public Projectile ProjectilePrefab => m_ProjectilePrefab;

    [SerializeField] private float m_RateOfFire;
    public float RateOfFire => m_RateOfFire;

    [SerializeField] private float spreadShootRange;
    public float SpreadShootRange => spreadShootRange;

    [SerializeField] private float spreadShootDistanceFactor;
    public float SpreadShootDistanceFactor => spreadShootDistanceFactor;

    [SerializeField] private int m_EnergyUsage;
    public int EnergyUsage => m_EnergyUsage;

    [SerializeField] private int energyAmountToStartFire;
    public int EnergyAmountToStartFire => energyAmountToStartFire;

    [SerializeField] private float energyRegenPerSecond;
    public float EnergyRegenPerSecond => energyRegenPerSecond;

    [SerializeField] private int m_AmmoUsage;
    public int AmmoUsage => m_AmmoUsage;

    [SerializeField] private AudioClip m_LaunchSFX;
    public AudioClip LaunchSFX => m_LaunchSFX;

}



