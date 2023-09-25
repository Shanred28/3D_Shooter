using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public enum SpawnMode
    {
        Start,
        Loop
    }

    [SerializeField] private Drone m_EntityPrefab;
    [SerializeField] private CubeArea m_Area;
    [SerializeField] private SpawnMode m_SpawnMode;
    [SerializeField] private int m_NumSpawns;
    [SerializeField] private float m_RespawnTime;

    [Header("Characteristic Entity Spawn")]
    [SerializeField] private CubeArea areaPatrol;
    [SerializeField] private float timeGetChangeTargetPoint;

        private float m_Timer;

        private void Start () 
        {
            if (m_SpawnMode == SpawnMode.Start)
            {
                SpawnEntities();
            }

            m_Timer = m_RespawnTime;
        }

        private void Update() 
        { 
            if(m_Timer > 0)
                m_Timer -= Time.deltaTime;

            if (m_Timer < 0 && m_SpawnMode == SpawnMode.Loop)
            {
                SpawnEntities();

                m_Timer = m_RespawnTime;
            }
        }

        private void SpawnEntities()
        {

                Drone e = Instantiate(m_EntityPrefab);
/*                e.isPatrul = true;
        e.areaPatrol = areaPatrol;
        e.timeGetChangeTargetPoint = timeGetChangeTargetPoint;*/
                e.transform.position = m_Area.GetRandomInsadeZone();

        }
    }
