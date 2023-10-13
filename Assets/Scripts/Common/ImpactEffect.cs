using UnityEngine;

public enum ImapctType
{
    NoHoles,
    Default
}

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float m_Lifetime;
    [SerializeField] private GameObject _hole;

    private float m_Timer;

    private void Update()
    {
        if(m_Timer < m_Lifetime)
            m_Timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }
}


