using UnityEngine;

public class Pickup_EnergyCell : TriggerInteractAction
{
    protected override void OnStartAction(GameObject owner)
    {
        base.OnStartAction(owner);
    }
    protected override void OnEndAction(GameObject owner)
    {
         Player player = owner.transform.root.GetComponent<Player>();

        if (player != null)
            player.AddEnergy();

        Destroy(gameObject);
        base.OnEndAction(owner);
    }
}
