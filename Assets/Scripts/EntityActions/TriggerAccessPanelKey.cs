using UnityEngine;

public class TriggerAccessPanelKey : TriggerInteractAction
{
    protected override void OnTriggerEnter(Collider other)
    {

        if(Player.Instance.CardRed == true)
           base.OnTriggerEnter(other);
    }
}
