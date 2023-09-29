using UnityEngine;

public class PatrolPathNode : MonoBehaviour
{
    [SerializeField] private float _idleTime;
    public float IdleTime => _idleTime;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,0.5f);
    }
#endif
}
