using UnityEngine;

public class TriggerAccessPanelKeyBlue : TriggerInteractAction
{
    protected override void OnTriggerEnter(Collider other)
    {

        if (Player.Instance.CardBlue == true)
            base.OnTriggerEnter(other);
    }
}
