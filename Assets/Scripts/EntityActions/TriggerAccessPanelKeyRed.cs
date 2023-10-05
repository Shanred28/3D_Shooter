using UnityEngine;

public class TriggerAccessPanelKeyRed : TriggerInteractAction
{
    protected override void OnTriggerEnter(Collider other)
    {

        if(Player.Instance.CardRed == true)
           base.OnTriggerEnter(other);
    }
}
