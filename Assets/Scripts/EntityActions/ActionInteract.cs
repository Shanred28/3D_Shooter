using System;
using UnityEngine;

public enum InteractType
{ 
    PickupItem,
    EnterCodeAccessPanel,
    ClimbingLadder,
    UseVehicle

}

[Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private Transform interactTransform;

    public Transform InteractTransform => interactTransform;
}

public class ActionInteract : EntityContextAction
{
    [SerializeField] protected Transform owner;

    [SerializeField] private InteractType interactType;
    public InteractType InteractType => interactType;

    protected new ActionInteractProperties Properties;
    public override void SetProperties(EntityActionProperties prop)
    {
        Properties = prop as ActionInteractProperties;
    }
}
