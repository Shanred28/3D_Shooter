using UnityEngine;

public class ColliderViewpoints : MonoBehaviour
{
    private enum ColliderType
    { 
       Character
    }

    [SerializeField] private ColliderType _colliderType;
    [SerializeField] private Collider _collider;

    private Vector3[] _points;

    private void Start()
    {
        if (_colliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }
    }

    private void Update()
    {
        if (_colliderType == ColliderType.Character)
        {
            CalcPointForCharacterController(_collider as CharacterController);
        }
    }


    // Public API
    public bool IsVisableFromPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            float angle = Vector3.Angle(_points[i] - point, eyeDir );
            float dist = Vector3.Distance(_points[i], point);

            if (angle <= viewAngle * 0.5f && dist <= viewDistance)
            {
                RaycastHit hit;

                Debug.DrawLine(point, _points[i], UnityEngine.Color.blue);
                if (Physics.Raycast(point, (_points[i] - point).normalized, out hit, viewDistance * 2) == true)
                {
                    if (hit.collider == _collider)
                    { 
                      return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsVisablePeripheralPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            float angle = Vector3.Angle(_points[i] - point, eyeDir);
            float dist = Vector3.Distance(_points[i], point);

            if (angle <= viewAngle * 0.5f && dist <= viewDistance)
            {
                RaycastHit hit;

                Debug.DrawLine(point, _points[i], UnityEngine.Color.green);
                if (Physics.Raycast(point, (_points[i] - point).normalized, out hit, viewDistance * 2) == true)
                {
                    if (hit.collider == _collider)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    [ContextMenu("UpdateViewPoints")]
    private void UpdateViewPoints()
    { 
        if(_collider == null) return;

        _points = null;

        if (_colliderType == ColliderType.Character)
        { 
            UpdatePointsForCharacterController();
        }
    }

    private void UpdatePointsForCharacterController()
    { 
        if(_points == null)
            _points = new Vector3[4];

        CharacterController collider = _collider as CharacterController;
        CalcPointForCharacterController(collider);
    }

    private void CalcPointForCharacterController(CharacterController collider)
    {
        _points[0] = collider.transform.position + collider.center + collider.transform.up * collider.height * 0.3f;
        _points[1] = collider.transform.position + collider.center - collider.transform.up * collider.height * 0.3f;
        _points[2] = collider.transform.position + collider.center + collider.transform.right * collider.radius * 0.4f;
        _points[3] = collider.transform.position + collider.center - collider.transform.right * collider.radius * 0.4f;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(_points == null) return;

        Gizmos.color = UnityEngine.Color.blue;

        for (int i = 0; i < _points.Length; i++)
            Gizmos.DrawSphere(_points[i], 0.1f);
    }
#endif
}
