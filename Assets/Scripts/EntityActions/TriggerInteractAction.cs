using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerInteractAction : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnInteract;
    public UnityEvent EventOnInteract => eventOnInteract;

    [SerializeField] private InteractType interactType;

    [SerializeField] private int interactAmount;

    [SerializeField] private ActionInteractProperties actionInteractProperties;

    protected ActionInteract actionInteract;

    public GameObject owner;

    protected virtual void InitActionProperties()
    {
        actionInteract.SetProperties(actionInteractProperties);
    }
    protected virtual void OnStartAction(GameObject owner) { }
    protected virtual void OnEndAction(GameObject owner) { }

    private void OnTriggerEnter(Collider other)
    {
        if (interactAmount == 0) return;

        if (other.transform.root.TryGetComponent(out EntityActionCollector actionCollector))
        {
            actionInteract = GetActionInteract(actionCollector);

            if (actionInteract != null)
            {
                InitActionProperties();
                actionInteract.IsCanStart = true;

                actionInteract.EventOnStart.AddListener(ActionStarted);
                actionInteract.EventOnEnd.AddListener(AtionEnded);

                owner = other.gameObject;
            }
        }
    }

    protected void OnTriggerExit(Collider other) 
    {
        if (interactAmount == 0 || interactAmount != - 1) return;

        if (other.transform.root.TryGetComponent(out EntityActionCollector actionCollector))
        {
            actionInteract = GetActionInteract(actionCollector);

            if (actionInteract != null)
            {
                actionInteract.IsCanStart = false;

                actionInteract.EventOnStart.RemoveListener(ActionStarted);
                actionInteract.EventOnEnd.RemoveListener(AtionEnded);
            }
        }
    }

    private void ActionStarted()
    {
        OnStartAction(owner);
    }

    private void AtionEnded()
    {
        actionInteract.IsCanStart = false;

        actionInteract.EventOnStart.RemoveListener(ActionStarted);
        actionInteract.EventOnEnd.RemoveListener(AtionEnded);

        eventOnInteract?.Invoke();

        interactAmount--;
        OnEndAction(owner);
    }

    private ActionInteract GetActionInteract(EntityActionCollector actionCollector)
    {
        List<ActionInteract> actions = actionCollector.GetActionList<ActionInteract>();

        foreach (ActionInteract action in actions)
        {
            if (interactType == action.InteractType)
            {
                return action;
            }
        }
        return null;
    }
}
