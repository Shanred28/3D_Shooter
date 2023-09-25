using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float rifleRunSpeed;
    [SerializeField] private float rifleSprintSpeed;
    [SerializeField] private float amimingWalkSpeed;
    [SerializeField] private float amimingRanSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float accelerationRate;

    [Header("State")]
    [SerializeField] private float crouchHeight;

    private bool isAiming;
    public bool IsAiming => isAiming;
    private bool isJump;
    public bool IsJump => isJump;
    private bool isCrouch;
    public bool IsCrouch => isCrouch;
    private bool isSprint;
    public bool IsSprint => isSprint;
    private float distanceToGround;
    public float DistanceToGround => distanceToGround;
    public bool IsGrounded => distanceToGround < 0.01f;

    private float baseCharacterHeight;
    private float baseCharacterOffsetCenter;

    // Controll action animations
    [SerializeField] private EntityActionCollector targetActionCollector;
    private bool isActionAnimation;
    public bool IsActionAnimation => isActionAnimation;


    private CharacterController characterController;
    public Vector3 TargetDirectionControl;
    public Vector3 DirectionControl;
    private Vector3 movementDirections;

   

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        baseCharacterHeight = characterController.height;
        baseCharacterOffsetCenter = characterController.center.y ;
    }

    private void Update()
    {
/*        if (prepare == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMoveToInteractPoint, Time.deltaTime * 5);

            if(transform.position == targetMoveToInteractPoint)
                prepare = false;

        }*/
        Move();
        UpdateDistanceToGround();
    }

    private void Move()
    {
        DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionControl, Time.deltaTime * accelerationRate);
        /*        if (prepare == false)
                {
                    DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionControl, Time.deltaTime * accelerationRate);
                }
                else
                {
                    DirectionControl = Vector3.MoveTowards(DirectionControl, targetMoveToInteractPoint, Time.deltaTime * accelerationRate);
                    if(transform.position == DirectionControl)
                        prepare = false;
                }*/
        /*if (prepare == true)
        {
            DirectionControl = Vector3.MoveTowards(DirectionControl, targetMoveToInteractPoint, Time.deltaTime * accelerationRate);
            movementDirections = DirectionControl * GetCurrentSpeedByState();
            movementDirections = transform.TransformDirection(movementDirections);
            movementDirections += Physics.gravity * Time.deltaTime;
            characterController.Move(movementDirections * Time.deltaTime);
            if(transform.position == targetMoveToInteractPoint)
                prepare = false;
            return;
        }
        else
            DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionControl, Time.deltaTime * accelerationRate);*/


        if (IsGrounded == true)
        {
            movementDirections = DirectionControl * GetCurrentSpeedByState();
            if (isJump== true)
            {
                movementDirections.y = jumpSpeed;
                isJump =false;
            }

            movementDirections = transform.TransformDirection(movementDirections);
        }

        movementDirections += Physics.gravity * Time.deltaTime;
        characterController.Move(movementDirections * Time.deltaTime);
    }

    public void Jump()
    {
        if (IsGrounded == false) return;
        if (isAiming == true || isCrouch == true) return;

        isJump = true;
    }

    public void Aiming()
    {
        isAiming = true;
    }
    public void UnAiming()
    {
        isAiming = false;
    }

    public void Crouch()
    {
        if (IsGrounded == false) return;
        if (isSprint == true) return;
        isCrouch = true;
        characterController.height = crouchHeight;
        characterController.center = new Vector3(0, 0.5f, 0);
    }
    public void UnCrouch()
    {
        isCrouch = false;
        characterController.height = baseCharacterHeight;
        characterController.center = new Vector3(0, baseCharacterOffsetCenter, 0);
    }

    public void Sprint()
    {
        if (IsGrounded == false) return;
        if (isCrouch == true) return;
        isSprint = true;
    }

    public void UnSprint()
    {
        isSprint = false;
    }

    public float GetCurrentSpeedByState()
    {
        if (isCrouch)
            return crouchSpeed;
        if (isAiming)
        {
            if (isSprint == true)
                return rifleSprintSpeed;
            else
                return rifleRunSpeed;
        }

        if (isAiming == false)
        {
            if (isSprint == true)
                return rifleSprintSpeed;
            else
                return rifleRunSpeed;
        }

        return rifleRunSpeed;
    }

    public void MoveActionPoint()
    {
        List<EntityContextAction> actionsList = targetActionCollector.GetActionList<EntityContextAction>();

        for (int i = 0; i < actionsList.Count; i++)
        {
            actionsList[i].StartAction();
           
        }
    }

    private Vector3 targetMoveToInteractPoint;
    private bool prepare;
    public void PreapreAction()
    {
/*        List<EntityContextAction> actionsList = targetActionCollector.GetActionList<EntityContextAction>();

        for (int i = 0; i < actionsList.Count; i++)
        {
            actionsList[i].   StartAction();
        }*/
        
       // StartCoroutine(MoveTo(targetInteractAction));
        
    }

    private void UpdateDistanceToGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, 1000) == true)
        {
            distanceToGround = Vector3.Distance(transform.position, hit.point);
        }
    }

    private float dist;
    private bool istarget = false;
    IEnumerator MoveTo( Vector3 target)
    {
        dist = Vector3.Distance(transform.position, target);
        while (istarget == false) 
        {
            DirectionControl = Vector3.MoveTowards(transform.position, target, Time.deltaTime * accelerationRate);
            movementDirections = DirectionControl * GetCurrentSpeedByState();
            movementDirections = transform.TransformDirection(movementDirections);
            movementDirections += Physics.gravity * Time.deltaTime;
            characterController.Move(movementDirections * Time.deltaTime);
            transform.LookAt(target);
            dist = Vector3.Distance(transform.position, target);
            if(dist < 1)
                istarget = true;
        }
        
        
        
        print("startMove");
        yield return new WaitForSeconds(15f);
        print("endMove");
    }
}
