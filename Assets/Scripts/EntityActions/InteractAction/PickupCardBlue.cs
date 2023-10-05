using UnityEngine;

public class PickupCardBlue : TriggerInteractAction
{
    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);
        Player.Instance.AddCardBlue();
        Destroy(gameObject);
    }
}
