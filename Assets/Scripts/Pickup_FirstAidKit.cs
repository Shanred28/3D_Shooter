using UnityEngine;

public class Pickup_FirstAidKit : TriggerInteractAction
{
    protected override void OnStartAction(GameObject owner)
    {
      base.OnStartAction(owner);
    }
    protected override void OnEndAction(GameObject owner)
    {
        
        Destructible des = owner.transform.root.GetComponent<Destructible>();

        if(des != null)
            des.HealFull();

        Destroy(gameObject);
        base.OnEndAction(owner);
    }
}
