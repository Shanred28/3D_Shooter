using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private ImapctType type;
    public ImapctType Type => type;

    [ContextMenu("AddToAllObject")]
    public void AddToAllObject()
    {
        Transform[] allTransform = FindObjectsOfType<Transform>();

        for (int i = 0; i < allTransform.Length; i++)
        {
            if (allTransform[i].GetComponent<Collider>() != null)
            {
                if (allTransform[i].GetComponent<Surface>() == null)
                {
                    allTransform[i].gameObject.AddComponent<Surface>();
                }
            }
        }
    }
}
