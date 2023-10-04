using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerInteractAction : MonoBehaviour
{
    [SerializeField] private UnityEvent eventStartInteract;
    public UnityEvent EventStartInteract => eventStartInteract;

    [SerializeField] private UnityEvent eventEndInteract;

    [SerializeField] private InteractType interactType;

    [SerializeField] private int interactAmount;

    [SerializeField] protected ActionInteractProperties _actionInteractProperties;
    public ActionInteractProperties InteractProperties => _actionInteractProperties;

    protected ActionInteract actionInteract;


    protected CharacterMovement characterMovement;
    protected GameObject owner;

    protected virtual void InitActionProperties()
    {
        actionInteract.SetProperties(_actionInteractProperties);
    }
    protected virtual void OnStartAction(GameObject owner) { }
    protected virtual void OnEndAction(GameObject owner) { }

    protected virtual  void OnTriggerEnter(Collider other)
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
                actionInteract.EventOnEnd.AddListener(ActionEnded);

                owner = other.gameObject;
                characterMovement = owner.GetComponent<CharacterMovement>();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) 
    {
        //if (interactAmount == 0 || interactAmount != - 1) return;
        if (other.transform.root.TryGetComponent(out EntityActionCollector actionCollector))
        {
            actionInteract = GetActionInteract(actionCollector);
            
            if (actionInteract != null)
            {
                actionInteract.IsCanStart = false;              

                actionInteract.EventOnStart.RemoveListener(ActionStarted);
                actionInteract.EventOnEnd.RemoveListener(ActionEnded);
            }
        }
    }

    private void ActionStarted()
    {
        OnStartAction(owner);
        interactAmount--;
        eventStartInteract?.Invoke();
        
    }

    private void ActionEnded()
    {
        actionInteract.IsCanStart = false;
        actionInteract.IsCanEnd = false;

        eventEndInteract?.Invoke();
        OnEndAction(owner);
        actionInteract.EventOnStart.RemoveListener(ActionStarted);
        actionInteract.EventOnEnd.RemoveListener(ActionEnded);
     
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
