using UnityEngine;

public class ColliderViewer : MonoBehaviour
{
    [SerializeField] private float _viewAngle;
    [SerializeField] private float _viewAnglePeripheral;
    [SerializeField] private float _viewDistance;
    [SerializeField] private float _viewHeight;

    // Public API
    public bool IsObjectVisible(GameObject target)
    { 
        ColliderViewpoints viewPoints = target.GetComponent<ColliderViewpoints>();
        if(viewPoints == false) return false;
        return viewPoints.IsVisableFromPoint(transform.position + new Vector3(0, _viewHeight, 0), transform.forward, _viewAngle,_viewDistance);
    }

    public bool IsObjectPeripheralVisible(GameObject target)
    {
        ColliderViewpoints viewPoints = target.GetComponent<ColliderViewpoints>();
        if (viewPoints == false) return false;
        return viewPoints.IsVisablePeripheralPoint(transform.position + new Vector3(0, _viewHeight, 0), transform.forward, _viewAnglePeripheral, _viewDistance);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, _viewHeight, 0), transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, _viewAngle, 0,_viewDistance,1);
    }
#endif
}
