using System.Collections.Generic;
using UnityEngine;
using static AiAlienSoldier;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Drone))]
public class AIDron : MonoBehaviour
{
    [SerializeField] private CubeArea _movementArea;

    [SerializeField] private ColliderViewer _colliderViewer;

    private Drone _drone;
    private Vector3 _movementTargetPos;
    private Transform _shootTarget;

    private bool isAiDron = true;

    private void Start()
    {
        _drone = GetComponent<Drone>();
        _drone.EventOnDeath.AddListener(OnDroneDeatch);
        _drone.OnGetDamage += OnGetDamage;
    }

    private void Update()
    {
        if(isAiDron)
        UpdateAI();
    }

    private void OnDestroy()
    {
        _drone.EventOnDeath.RemoveListener(OnDroneDeatch);
        _drone.OnGetDamage -= OnGetDamage;
    }

    // Handlers
    private void OnGetDamage(Destructible other)
    {
        ActionAssignTargetAllTeamMember(other.transform);
    }

    private void OnDroneDeatch()
    { 
       enabled = false;
    }

    //AI
    private void UpdateAI()
    {
        ActionFindShootingTarget();

        ActionMove();   
        
        ActionFire();
    }

    // Action

    private void ActionFindShootingTarget()
    {
       Transform potentionalTarget = FindShootTarget();

        if (potentionalTarget != null)
        { 
            ActionAssignTargetAllTeamMember(potentionalTarget);
        }
    }

    private void ActionMove()
    {
        if (transform.position == _movementTargetPos)
        {
            _movementTargetPos = _movementArea.GetRandomInsadeZone();
        }

        if (Physics.Linecast(transform.position, _movementTargetPos) == true)
        {
            _movementTargetPos = _movementArea.GetRandomInsadeZone();
        }
        _drone.MoveTo(_movementTargetPos);

        if (_shootTarget != null)
        {
            _drone.LookAt(_shootTarget.position);
        }
        else
            _drone.LookAt(_movementTargetPos);
    }

    private void ActionFire()
    {
        if (_shootTarget != null)
        {
            _drone.Fire(_shootTarget.position);
        }
    }

    //Methods
    public void SetShootTarget(Transform target)
    {
        _shootTarget = target;
    }

    private Transform FindShootTarget()
    {
        List<Destructible> targets = Destructible.GetAllNonTeamMember(_drone.TeamId);

        for (int i = 0; i < targets.Count; i++)
        {
            if (_colliderViewer.IsObjectVisible(targets[i].gameObject) == true)
                return targets[i].transform;
        }

        return null;
    }

    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMember(_drone.TeamId);

        foreach (Destructible dest in team)
        {
            AIDron ai = dest.transform.root.GetComponent<AIDron>();

            if (ai != null && ai.enabled == true)
            {
                ai.SetShootTarget(other);
            }
        }
    }


    // Public API
    public void DisableAI()
    {
        isAiDron = false;
    }
}
