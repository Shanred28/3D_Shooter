using UnityEngine;

public class Pickup_FirstAidKit : TriggerInteractAction
{
    protected override void OnStartAction(GameObject owner)
    {
        var player = owner.transform.root.GetComponent<CharacterMovement>();

     //   StartCoroutine(Fade(player, owner.transform.position, owner));
       // player.PreapreAction(owner.transform.position);
        //base.OnStartAction(owner);
    }

    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);
        Destructible des = owner.transform.root.GetComponent<Destructible>();

        if(des != null)
            des.HealFull();

        Destroy(gameObject);
    }

/*    IEnumerator Fade(CharacterMovement player, Vector3 target,GameObject game )
    {
        player.PreapreAction(target);
        
        base.OnStartAction(game);
        print("end");
        yield return null;
    }*/
}
