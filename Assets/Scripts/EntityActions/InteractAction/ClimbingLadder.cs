using UnityEngine;

public class ClimbingLadder : TriggerInteractAction
{
    [SerializeField] private Transform pointClimbing;

    protected override void OnStartAction(GameObject owner)
    {
        characterMovement.IsClimbing = true;
        //characterMovement.ClimbingMove(pointClimbing.position);
        base.OnStartAction(owner);
    }

    protected override void OnEndAction(GameObject owner)
    {
        characterMovement.IsClimbing = false;
        base.OnEndAction(owner);
    }
}
