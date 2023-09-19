using UnityEngine;

public class Drone : Destructible
{
    [Header("Main")]
    [SerializeField] private Transform mainMesh;

    [Header("View")]
    [SerializeField] private GameObject[] meshComponents;

    [SerializeField] private Renderer[] mashRenderers;
    [SerializeField] private Material[] materialsDead;

    [Header("Movement")]
    [SerializeField] private float hoverAmplitude;
    [SerializeField] private float hoverSpeed;

    [Header("Patrul")]
    [SerializeField] public bool isPatrul =false;
    [SerializeField] public CubeArea areaPatrol;
    [SerializeField] private float speedFly;
    [SerializeField] public float timeGetChangeTargetPoint;
    private Vector3 targetPointPatrol;
    private float timer;
    private void Update()
    {
        mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime, 0);

        if (isPatrul)
        {
            if (timer <= 0)
            {
                timer = timeGetChangeTargetPoint;
                targetPointPatrol = GetRandomPointPatrol();
            }
                
            timer -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPointPatrol, Time.deltaTime * speedFly);
            transform.LookAt(Vector3.Lerp(transform.position, targetPointPatrol, Time.deltaTime * speedFly) );
        }
    }

    protected override void OnDeath()
    {
        if (IsDestroy == false)
        {
            EventOnDeath?.Invoke();
            enabled = false;

            for (int i = 0; i < meshComponents.Length; i++)
            {
                if (meshComponents[i].GetComponent<Rigidbody>() == null)
                    meshComponents[i].AddComponent<Rigidbody>();
            }

            for (int i = 0; i < mashRenderers.Length; i++)
            {
                mashRenderers[i].material = materialsDead[i];
            }
                IsDestroy = true;
        }
    }

    private Vector3 GetRandomPointPatrol()
    {
        return areaPatrol.GetRandomInsadeZone();
    }
}
