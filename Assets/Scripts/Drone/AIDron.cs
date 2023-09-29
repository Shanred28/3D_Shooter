using UnityEngine;

[RequireComponent(typeof(Drone))]
public class AIDron : MonoBehaviour
{
    [SerializeField] private CubeArea movementArea;
    [SerializeField] private float adngryDistance;

    private Drone drone;
    private Vector3 movementTargetPos;
    private Transform shootTarget;

    private bool isAiDron = true;

    private Transform player;
    private void Start()
    {
        drone = GetComponent<Drone>();
        drone.EventOnDeath.AddListener(OnDroneDeatch);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(isAiDron)
        UpdateAI();
    }

    private void OnDestroy()
    {
        drone.EventOnDeath.RemoveListener(OnDroneDeatch);
    }

    private void OnDroneDeatch()
    { 
       enabled = false;
    }

    //AI
    private void UpdateAI()
    {
        //Update movement position

        if (transform.position == movementTargetPos)
        {
            movementTargetPos = movementArea.GetRandomInsadeZone();
        }

        if (Physics.Linecast(transform.position, movementTargetPos) == true)
        { 
            movementTargetPos = movementArea.GetRandomInsadeZone();
        }

        //Finde Fire position.

        if (Vector3.Distance(transform.position, player.position) <= adngryDistance)
        { 
            shootTarget = player.transform;
        }

        //Move
        drone.MoveTo(movementTargetPos);

        if (shootTarget != null)
        {
            drone.LookAt(shootTarget.position);
        }
        else
            drone.LookAt(movementTargetPos);

        //Fire
        if (shootTarget != null)
        { 
            drone.Fire(shootTarget.position);
        }

    }

    public void DisableAI()
    {
        isAiDron = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, adngryDistance);
    }
#endif
}
