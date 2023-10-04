using UnityEngine;

public class PickupCardRed : TriggerInteractAction
{
    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);
        Player.Instance.AddCard();
        Destroy(gameObject);
    }
}
