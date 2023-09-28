using System;
using UnityEngine;

public enum InteractType
{ 
    PickupItem,
    EnterCodeAccessPanel,
    ClimbingLadder

}

[Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private Transform interactTransform;

    public Transform InteractTransform => interactTransform;
}

public class ActionInteract : EntityContextAction
{
    [SerializeField] private Transform owner;

    [SerializeField] private InteractType interactType;
    public InteractType InteractType => interactType;

    private ActionInteractProperties properties;
    public override void SetProperties(EntityActionProperties prop)
    {
        properties = prop as ActionInteractProperties;
    }

    public override void StartAction()
    {
        if (IsCanStart == false) return;
        base.StartAction();
     
    }

}
