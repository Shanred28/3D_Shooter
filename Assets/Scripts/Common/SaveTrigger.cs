using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out SceneSerializer sceneSerializer))
        { 
            if (sceneSerializer == null) return;

            sceneSerializer.SaveScene();
        }
    }
}
