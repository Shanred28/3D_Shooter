using UnityEngine;

public class VechicleUseTrigger : TriggerInteractAction
{

    [SerializeField] private ActionUseVehicleProperties useProperties;

    protected override void InitActionProperties()
    {
        actionInteract.SetProperties(useProperties);
    }
}
