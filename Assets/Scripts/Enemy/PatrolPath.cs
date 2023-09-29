using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private PatrolPathNode[] _nodes;

    private void Start()
    {
        UpdatePathNode();
    }

    [ContextMenu("Update Path Node")]

    private void UpdatePathNode()
    {
        _nodes = new PatrolPathNode[transform.childCount];

        for (int i = 0; i < _nodes.Length; i++)
        {
            _nodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
        }
    }

    public PatrolPathNode GetRandomPathNode()        
    {
        return _nodes[Random.Range(0, _nodes.Length)];
    }

    public PatrolPathNode GetNextNode(ref int index)
    {
        index = Mathf.Clamp(index,0,_nodes.Length - 1);

        index++;

        if(index >= _nodes.Length)
            index = 0;

        return _nodes[index];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(_nodes == null) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _nodes.Length - 1; i++)
        {
            Gizmos.DrawLine(_nodes[i].transform.position + new Vector3(0, 0.5f, 0), _nodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
        }

        Gizmos.DrawLine(_nodes[0].transform.position + new Vector3(0, 0.5f, 0), _nodes[_nodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
    }
#endif
}
