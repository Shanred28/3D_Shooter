using UnityEngine;

public class ShootingVehicleInputControl : VehicleInputControl
{
    [SerializeField]
    private CameraShooter m_CameraShooter;

    [SerializeField]
    private Transform m_AimPoint;

    protected override void Start()
    {
        base.Start();
        //m_CameraShooter = m_CameraShooter.Camera.GetComponent<CameraShooter>();
        m_AimPoint = Player.Instance.Aim;
    }

    protected override void Update()
    {
        base.Update();

       m_AimPoint.position = m_CameraShooter.Camera.transform.position +  m_CameraShooter.Camera.transform.forward * 30;

        if (Input.GetMouseButton(0))
        {
            m_CameraShooter.Shoot();
        }
    }
}
