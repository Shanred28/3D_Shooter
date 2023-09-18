using UnityEngine;

public class CubeArea : MonoBehaviour
{
    [SerializeField] private Vector3 movementArea;

    public Vector3 GetRandomInsadeZone()
    { 
        Vector3 result = transform.position;

        result.x += Random.Range(-movementArea.x / 2, movementArea.x / 2);
        result.y += Random.Range(-movementArea.y / 2, movementArea.y / 2);
        result.z += Random.Range(-movementArea.z / 2, movementArea.z / 2);

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, movementArea);
    }
#endif
}
