using UnityEngine;

public class Turret : Weapon
{
    [SerializeField]
    private Transform m_Base;

    [SerializeField]
    private Transform m_Gun;

    [SerializeField]
    private Transform m_Aim;

    [SerializeField]
    private float m_RotationLerpFactor;

    protected Quaternion BaseTargetRotation;
    protected Quaternion BaseRotation;
    protected Quaternion GunTargetRotation;
    protected Vector3 GunRotation;

    protected override void Update()
    {
        base.Update();

       // LookOnAim();
    }

    private void LookOnAim()
    {
        BaseTargetRotation = Quaternion.LookRotation(new Vector3(m_Aim.position.x, m_Gun.position.y, m_Aim.position.z) - m_Base.position);
        BaseRotation = Quaternion.RotateTowards(m_Base.localRotation, BaseTargetRotation, Time.deltaTime * m_RotationLerpFactor);
        m_Base.transform.LookAt(m_Aim);


        GunTargetRotation = Quaternion.LookRotation(m_Aim.position - m_Base.position);
        GunRotation = Quaternion.RotateTowards(m_Gun.rotation, GunTargetRotation, Time.deltaTime * m_RotationLerpFactor).eulerAngles;
        m_Gun.localRotation = BaseRotation * Quaternion.Euler(GunRotation.x, 0, 0);
    }

    public void SetAim(Transform aim)
    {
        m_Aim = aim;
    }

}
