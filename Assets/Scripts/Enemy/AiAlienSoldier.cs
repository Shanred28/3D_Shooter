using System.Collections;
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
        PersueTarget,
        SeekTarget
    }

    [SerializeField] private AIBehaviour _aiBehaviour;

    [SerializeField] private AlienSoldier _alienSoldier;
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private PatrolPath _patrolPath;

    [SerializeField] private ColliderViewer _colliderViewer;

    [SerializeField] private int _patrolPatchNodeIndex = 0;

    private NavMeshPath _navMeshPath;
    private PatrolPathNode _currentPathNode;

    private GameObject _potentionalTarget;
    private Transform _pursueTarget;
    private Vector3 _seekTarget;

    private void Start()
    {
        _potentionalTarget = Destructible.FindNearestNonTamMember(_alienSoldier)?.gameObject;
        _characterMovement.UpdatePosition = false;
        _navMeshPath = new NavMeshPath();
        StartBehaviour(_aiBehaviour);
    }

    private void Update()
    {
        SyncAgentAndCharacterMovement();
        UpdateAI();
    }

    private void UpdateAI()
    {
        ActionUpdateTarget();

        if (_aiBehaviour == AIBehaviour.Idle)
        {
            return;
        }

        if (_aiBehaviour == AIBehaviour.PersueTarget)
        {
            _agent.CalculatePath(_pursueTarget.position,_navMeshPath);
            _agent.SetPath(_navMeshPath);
        }

        if (_aiBehaviour == AIBehaviour.SeekTarget)
        {
            _agent.CalculatePath(_seekTarget, _navMeshPath);
            _agent.SetPath(_navMeshPath);

            if (AgentReachedDistination() == true)
            {
                StartBehaviour(AIBehaviour.PatrolRandom);
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
        if (_potentionalTarget == null) return;

        if (_colliderViewer.IsObjectVisible(_potentionalTarget) == true)
        {
            _pursueTarget = _potentionalTarget.transform;
            StartBehaviour(AIBehaviour.PersueTarget);
        }
        else
        {
            if (_pursueTarget != null)
            { 
                _seekTarget = _pursueTarget.position;
                _pursueTarget = null;
                StartBehaviour(AIBehaviour.SeekTarget);
            }
        }
    }


    //Behaviour
    private void StartBehaviour(AIBehaviour state)
    {
        if (state == AIBehaviour.Idle)
        { 
            _agent.isStopped = true;
        }

        if (state == AIBehaviour.PatrolRandom)
        {
            _agent.isStopped = false;
            SetDistinationByPathNode(_patrolPath.GetRandomPathNode());
        }

        if (state == AIBehaviour.CirclePatrol)
        {
            _agent.isStopped = false;
            SetDistinationByPathNode(_patrolPath.GetNextNode(ref _patrolPatchNodeIndex));
        }
        _aiBehaviour = state;
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

    IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
    {
        AIBehaviour previous = _aiBehaviour;
        _aiBehaviour = state;
        StartBehaviour(_aiBehaviour);

        yield return new WaitForSeconds(second);

        StartBehaviour(previous);
    }
}
