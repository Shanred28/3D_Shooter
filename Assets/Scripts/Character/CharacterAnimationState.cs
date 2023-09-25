using System;
using UnityEngine;

[Serializable]
public class CharacterAnimatorParametersName
{
    public string NormolizeMovementX;
    public string NormolizeMovementZ;
    public string Sprint;
    public string Crouch;
    public string Aiming;
    public string Ground;
    public string Jump;
    public string GroundSpeed;
    public string DistanceToGround;

}

[Serializable]
public class AnimationCrossFadeParametrs
{
    public string Name;
    public float Duration;
}


public class CharacterAnimationState : MonoBehaviour
{
    private const float INPUT_CONTROL_LERP = 10f;
    [SerializeField] private CharacterController targetCharacterController;
    [SerializeField] private CharacterMovement characterMovement;

    [SerializeField]
    [Space(5)]
    private CharacterAnimatorParametersName animatorParametersName;

    [SerializeField] private Animator targetAnimator;

    [SerializeField]
    [Header("Fades")]
    [Space(5)]
    private AnimationCrossFadeParametrs fallFade;

    [SerializeField] private float minDistanceToGroundByFall;

    [SerializeField] private AnimationCrossFadeParametrs jumpIdleFade;
    [SerializeField] private AnimationCrossFadeParametrs jumpMoveFade;

    private Vector3 inputControl;

    private void LateUpdate()
    {
        Vector3 movementSpeed = transform.InverseTransformDirection( targetCharacterController.velocity);
        inputControl = Vector3.MoveTowards(inputControl, characterMovement.TargetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP);
        targetAnimator.SetFloat(animatorParametersName.NormolizeMovementX,  inputControl.x);
        targetAnimator.SetFloat(animatorParametersName.NormolizeMovementZ,  inputControl.z);

        targetAnimator.SetBool(animatorParametersName.Sprint, characterMovement.IsSprint);
        targetAnimator.SetBool(animatorParametersName.Crouch, characterMovement.IsCrouch);
        targetAnimator.SetBool(animatorParametersName.Aiming, characterMovement.IsAiming);
        targetAnimator.SetBool(animatorParametersName.Ground, characterMovement.IsGrounded);

        Vector3 groundSpeed = targetCharacterController.velocity;
        groundSpeed.y = 0;
        targetAnimator.SetFloat(animatorParametersName.GroundSpeed, groundSpeed.magnitude);

        if (characterMovement.IsJump == true)
        {
            if (groundSpeed.magnitude <= 0.03f) 
            {
                CrossFade(jumpIdleFade);
            }

            if (groundSpeed.magnitude > 0.03f)
            {
                CrossFade(jumpMoveFade);
            }
        }

        if (characterMovement.IsGrounded == false)
        {
            targetAnimator.SetFloat(animatorParametersName.Jump, movementSpeed.y);

            if (movementSpeed.y < 0 && characterMovement.DistanceToGround > minDistanceToGroundByFall)
            {
                CrossFade(fallFade);
            }
            targetAnimator.SetFloat(animatorParametersName.Jump, movementSpeed.y);
        }
/*       else
            targetAnimator.SetFloat(animatorParametersName.Jump, movementSpeed.y);*/
        
        targetAnimator.SetFloat(animatorParametersName.DistanceToGround, characterMovement.DistanceToGround);
    }

    private void CrossFade(AnimationCrossFadeParametrs parametrs)
    {
        targetAnimator.CrossFade(parametrs.Name, parametrs.Duration);
    }


}
