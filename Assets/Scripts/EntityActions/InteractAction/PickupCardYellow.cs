using UnityEngine;

public class PickupCardYellow : TriggerInteractAction
{
    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);
        Player.Instance.AddCardYellow();
        Destroy(gameObject);
    }
}
