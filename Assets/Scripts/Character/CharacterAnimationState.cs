using UnityEngine;

public class CharacterAnimationState : MonoBehaviour
{
    private const float INPUT_CONTROL_LERP = 10f;
    [SerializeField] private CharacterController targetCharacterController;
    [SerializeField] private CharacterMovement characterMovement;

    [SerializeField] private Animator targetAnimator;
    private Vector3 inputControl;

    private void LateUpdate()
    {
        Vector3 movementSpeed = transform.InverseTransformDirection( targetCharacterController.velocity);
        inputControl = Vector3.MoveTowards(inputControl, characterMovement.TargetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP);
        targetAnimator.SetFloat("NormolizeMovementX",  inputControl.x);
        targetAnimator.SetFloat("NormolizeMovementZ",  inputControl.z);

        targetAnimator.SetBool("IsSprint", characterMovement.IsSprint);
        targetAnimator.SetBool("IsCrouch", characterMovement.IsCrouch);
        targetAnimator.SetBool("IsAiming", characterMovement.IsAiming);
        targetAnimator.SetBool("IsGrounded", characterMovement.IsGrounded);

        if (characterMovement.IsGrounded == false)
        {
            targetAnimator.SetFloat("Jump", movementSpeed.y);
        }
        else
            targetAnimator.SetFloat("Jump", movementSpeed.y);
        Vector3 groundSpeed = targetCharacterController.velocity;
        groundSpeed.y = 0;
        targetAnimator.SetFloat("GroundSpeed", groundSpeed.magnitude);
        targetAnimator.SetFloat("DistanceToGround", characterMovement.DistanceToGround);
    }


}
