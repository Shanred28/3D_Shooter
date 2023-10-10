using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestReachedTrigger : Quest
{
    [SerializeField] private GameObject _owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _owner) return;

        if(Complited != null)
           Complited.Invoke();
    }
}
