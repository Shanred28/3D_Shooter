using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestReachedTrigger : Quest
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _owner) return;

        if(Complited != null)
           Complited.Invoke();
    }
}
