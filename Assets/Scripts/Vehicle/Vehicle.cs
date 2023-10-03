using UnityEngine;

public class Vehicle : Destructible
{
    [SerializeField] protected float _maxSpeed;

    [SerializeField] protected LightVehicle _lightVehicle;

    [Header("Hud")]
    [SerializeField] protected GameObject hud;
    


    public virtual float LinearVelocity => 0;

    public float NormalizedLinearVelocity
    {
        get 
        {
            if (Mathf.Approximately(0, LinearVelocity) == true) return 0;

            return Mathf.Clamp01(LinearVelocity / _maxSpeed);
        }
    }

    protected Vector3 TargetInputControl;

    public void SetTargetControl(Vector3 control)
    {
        TargetInputControl = control.normalized;
    }

    [Header("Engine Sound")]
    [SerializeField] private AudioSource _engineSFX;
    [SerializeField] private float _engineSFXModifier;

    protected virtual void Update()
    {
        if(isStartDrive == true)
           UpdateEngineSFX();
    }

    private void UpdateEngineSFX()
    {
        if (_engineSFX != null)
        {
            _engineSFX.pitch = 1.0f + NormalizedLinearVelocity * _engineSFXModifier;
            _engineSFX.volume = 0.5f + NormalizedLinearVelocity;
        }
    }

    private bool isStartDrive;
    public virtual void OnStartDrive()
    {
        isStartDrive = true;
        _lightVehicle.OnLight();
        _engineSFX.Play();
    }

    public virtual void OffStartDrive()
    {
        isStartDrive = false;
        _lightVehicle.OffLight();
        _engineSFX.Stop();
    }
}
