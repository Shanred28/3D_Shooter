using UnityEngine;

public class TriggerAccessPanelKeyYellow : TriggerInteractAction
{
    protected override void OnTriggerEnter(Collider other)
    {

        if (Player.Instance.CardYellow == true)
            base.OnTriggerEnter(other);
    }
}
