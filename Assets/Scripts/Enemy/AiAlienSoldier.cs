using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAlienSoldier : MonoBehaviour
{
    public enum AIBehaviour
    { 
        Null,
        Idle,
        PatrolRandom,
        CirclePatrol,
        PursuetTarget,
        SeekTarget
    }

    public enum AlarmType
    { 
        Detected,
        Midle,
        Non
    }

    [SerializeField] private AIBehaviour _aiBehaviour;

    [SerializeField] private AlienSoldier _alienSoldier;
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PatrolPath _patrolPath;

    [SerializeField] private ColliderViewer _colliderViewer;

    [SerializeField] private float _aimingDistance;

    [SerializeField] private int _patrolPatchNodeIndex = 0;

    [SerializeField] private UI_IndicatorVisable _indicatorVisable;

    private NavMeshPath _navMeshPath;
    private PatrolPathNode _currentPathNode;

    private GameObject _potentionalTarget;
    private Transform _pursuetTarget;
    private Vector3 _seekTarget;

    private Timer timer;
    [SerializeField] private float timeDetectingPeripheral;

    private void Start()
    {
        _potentionalTarget = Player.Instance.gameObject;
        _characterMovement.UpdatePosition = false;

        _navMeshPath = new NavMeshPath();
        StartBehaviour(_aiBehaviour);
        timer = Timer.CreateTimer(timeDetectingPeripheral);
        _alienSoldier.OnGetDamage += OnGetDamage;
    }

    private void Update()
    {
        SyncAgentAndCharacterMovement();
        UpdateAI();
    }

    private void OnDestroy()
    {
        _alienSoldier.OnGetDamage -= OnGetDamage;
    }

    // Handler
    private void OnGetDamage(Destructible other)
    {
        if (other.TeamId != _alienSoldier.TeamId)
        {
            if (_colliderViewer.IsObjectVisible(_potentionalTarget) == true)
            {
                ActionAssignTargetAllTeamMember(other.transform);
            }
            else
            {
                _seekTarget = other.transform.position;
                ActionAssignTargetAllTeamMember(other.transform);
                StartBehaviour(AIBehaviour.SeekTarget);
            }           
        }
    }

    // AI
    private void UpdateAI()
    {
        ActionUpdateTarget();

        if (_aiBehaviour == AIBehaviour.Idle)
        {
            return;
        }

        if (_aiBehaviour == AIBehaviour.PursuetTarget)
        {
            _agent.CalculatePath(_pursuetTarget.position,_navMeshPath);
            _agent.SetPath(_navMeshPath);

            if (Vector3.Distance(transform.position, _pursuetTarget.position) <= _aimingDistance)
            {
                _characterMovement.Aiming();
                _alienSoldier.Fire(_pursuetTarget.position + new Vector3(0, 0.8f, 0));
            }
            else
                _characterMovement.UnAiming();
        }

        if (_aiBehaviour == AIBehaviour.SeekTarget)
        {
            _agent.CalculatePath(_seekTarget, _navMeshPath);
            _agent.SetPath(_navMeshPath);

            if (AgentReachedDistination() == true)
            {
                //StartBehaviour(AIBehaviour.PatrolRandom);
                _pointCentreAreaSeek = _seekTarget;
               StartCoroutine( SeekAreaTarget());
            }
        }

        if (_aiBehaviour == AIBehaviour.PatrolRandom)
        {
            if (AgentReachedDistination() == true)
            {
                StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle,_currentPathNode.IdleTime));
            }          
        }

        if (_aiBehaviour == AIBehaviour.CirclePatrol)
        {
            if (AgentReachedDistination() == true)
            {
                StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, _currentPathNode.IdleTime));
            }
        }
    }

    // Action
    private void ActionUpdateTarget()
    {
        if (_potentionalTarget == null)
        {
            timer.Restart(timeDetectingPeripheral);
            return;
        } 

        if (_colliderViewer.IsObjectPeripheralVisible(_potentionalTarget) == true)
        {
            if (_colliderViewer.IsObjectVisible(_potentionalTarget) == true)
            {
                _pursuetTarget = _potentionalTarget.transform;
                ActionAssignTargetAllTeamMember(_pursuetTarget);
                Alarm(AlarmType.Detected);
                timer.Stop();
            }
            else
            {
                timer.Play();
                _indicatorVisable.SetAllarmFill(timer.CurrentTime, timeDetectingPeripheral);
                if (timer.IsComplited)
                {
                    _pursuetTarget = _potentionalTarget.transform;
                    ActionAssignTargetAllTeamMember(_pursuetTarget);
                    Alarm(AlarmType.Detected);
                    timer.Stop();
                }
            }           
        }
        else
        {
            timer.Restart(timeDetectingPeripheral);
            if (_pursuetTarget != null)
            { 
                _seekTarget = _pursuetTarget.position;
                Alarm(AlarmType.Midle);
                _pursuetTarget = null;
                StartBehaviour(AIBehaviour.SeekTarget);
                timer.Stop();
            }
        }        
    }

    [SerializeField] private float _radiusHearing;
    public void ApplyHearling(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < _radiusHearing)
        {
            _seekTarget = target;
            StartBehaviour(AIBehaviour.SeekTarget);
        }
    }


    //Behaviour
    private void StartBehaviour(AIBehaviour state)
    {
        if (_alienSoldier.IsDestroy == true) return;

        if (state == AIBehaviour.Idle)
        { 
            _agent.isStopped = true;
            _characterMovement.UnAiming();
        }

        if (state == AIBehaviour.PatrolRandom)
        {
            _agent.isStopped = false;
            _characterMovement.UnAiming();
            SetDistinationByPathNode(_patrolPath.GetRandomPathNode());
        }

        if (state == AIBehaviour.CirclePatrol)
        {
            _agent.isStopped = false;
            _characterMovement.UnAiming();
            SetDistinationByPathNode(_patrolPath.GetNextNode(ref _patrolPatchNodeIndex));
        }

        if (state == AIBehaviour.PursuetTarget)
        {
            _agent.isStopped = false;
            
        }

        if (state == AIBehaviour.SeekTarget)
        {
            _agent.isStopped = false;
            _characterMovement.UnAiming();
        }
        _aiBehaviour = state;
    }

    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMember(_alienSoldier.TeamId);

        foreach (Destructible dest in team)
        { 
            AiAlienSoldier ai = dest.transform.root.GetComponent<AiAlienSoldier>();

            if (ai != null && ai.enabled == true)
            {
                ai.SetPursueTarget(other);
                ai.StartBehaviour(AIBehaviour.PursuetTarget);
            }
        }
    }

    public void SetPursueTarget(Transform target)
    { 
       _pursuetTarget = target;
    }

    [SerializeField] private float _radiusAreaSeek;
    private Vector3 _pointCentreAreaSeek;
    public Vector3 GetPointRandomSeekArea()
    {
        Vector3 result = _pointCentreAreaSeek;

        result.x = Random.Range(result.x - _radiusAreaSeek, result.x +  _radiusAreaSeek );
        result.z = Random.Range(result.z - _radiusAreaSeek, result.z +  _radiusAreaSeek );
       

        return result;
    }

    private void SetDistinationByPathNode(PatrolPathNode node)
    {
        _currentPathNode = node;
        _agent.CalculatePath(node.transform.position, _navMeshPath);

        _agent.SetPath(_navMeshPath);
    }

    private void SyncAgentAndCharacterMovement()
    {
        _agent.speed = _characterMovement.CurrentSpeed;

         float factor = _agent.velocity.magnitude / _agent.speed;
        _characterMovement.TargetDirectionControl = transform.InverseTransformDirection(_agent.velocity.normalized) * factor;
    }

    private bool AgentReachedDistination()
    {
        if (_agent.pathPending == false)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (_agent.hasPath == false || _agent.velocity.magnitude == 0.0f)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    private void Alarm(AlarmType alarmType)
    {

        switch (alarmType)
        {
            case AlarmType.Detected:
                _indicatorVisable?.Detected ();
                break;
            case AlarmType.Midle:
                _indicatorVisable?.MidleDetected();
                break;
            case AlarmType.Non:
                _indicatorVisable?.NonDetected();
                break;
        }
    }

    IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
    {
        AIBehaviour previous = _aiBehaviour;
        _aiBehaviour = state;
        StartBehaviour(_aiBehaviour);

        yield return new WaitForSeconds(second);

        StartBehaviour(previous);
    }

    IEnumerator SeekAreaTarget()
    {
        _seekTarget = GetPointRandomSeekArea();
        _agent.CalculatePath(_seekTarget, _navMeshPath);
        _agent.SetPath(_navMeshPath);


        yield return new WaitForSeconds(2);

        _seekTarget = GetPointRandomSeekArea();
        _agent.CalculatePath(_seekTarget, _navMeshPath);
        _agent.SetPath(_navMeshPath);

        yield return new WaitForSeconds(2);

        _seekTarget = GetPointRandomSeekArea();
        _agent.CalculatePath(_seekTarget, _navMeshPath);
        _agent.SetPath(_navMeshPath);

        yield return new WaitForSeconds(2);
        Alarm( AlarmType.Non);
        StartBehaviour(AIBehaviour.PatrolRandom);
    }
}
