using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : MonoBehaviour
{
    public UnityAction<Vector3> Land;

    [Header("Movement")]
    [SerializeField] private float rifleRunSpeed;
    [SerializeField] private float rifleSprintSpeed;
    [SerializeField] private float amimingWalkSpeed;
    [SerializeField] private float amimingRanSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float _flyFallDead;

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
    public bool IsGrounded => distanceToGround < 0.09f;

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

    public bool UpdatePosition;
    public float CurrentSpeed => GetCurrentSpeedByState();

    public EntityContextAction action;
    private Vector3 targetMoveToInteractPoint;
    private bool IsMoveAction = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        baseCharacterHeight = characterController.height;
        baseCharacterOffsetCenter = characterController.center.y ;
        characterController.enabled = true;

    }

    private void Update()
    {
           

        if (IsClimbing == false)
        {
            TargetControlMove();
            UpdateDistanceToGround();

        }
        FallingFromHeight();
    }

    private void FixedUpdate()
    {
        if (IsClimbing == true)
            ClimbingMove();

       
           Move();
    }

    // Движение в FixedUpdate.
    private void Move()
    {
        if (IsGrounded == true)
        {
            movementDirections = DirectionControl * GetCurrentSpeedByState();
            if (isJump == true)
            {
                movementDirections.y = jumpSpeed;
                isJump = false;
            }

            movementDirections = transform.TransformDirection(movementDirections);
        }
        movementDirections += Physics.gravity * Time.fixedDeltaTime;
        if (UpdatePosition == true)
            characterController.Move(movementDirections * Time.fixedDeltaTime);

        if (characterController.isGrounded == true && Mathf.Abs(movementDirections.y) > 2)
        {
            if (Land != null)
                Land.Invoke(movementDirections);
        }
    }

    // Вычесления направления в Update.
    private void TargetControlMove()
    {
        if (IsMoveAction == false)
        {
            DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionControl, Time.deltaTime * accelerationRate);
        }
        else
        {
            var dist = Vector3.Distance(transform.position, targetMoveToInteractPoint);
            transform.LookAt(targetMoveToInteractPoint);

            if (dist > 1f)
            {
                DirectionControl = Vector3.MoveTowards(DirectionControl, targetMoveToInteractPoint.normalized, Time.deltaTime * accelerationRate);
            }
            else
                IsMoveAction = false;
            /*            else
                        {
                            List<EntityContextAction> actionsList = targetActionCollector.GetActionList<EntityContextAction>();

                            for (int i = 0; i < actionsList.Count; i++)
                            {

                                actionsList[i].StartAction();
                            }

                            IsMoveAction = false;
                        }*/

        }
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
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + characterController.height, transform.position.z), Vector3.up, out hit, characterController.height) == true)
            isCrouch = true;
        else
        {
            isCrouch = false;
            characterController.height = baseCharacterHeight;
            characterController.center = new Vector3(0, baseCharacterOffsetCenter, 0);
        }
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

    public void ClimbingMove(/*Vector3 target*/)
    {
        DirectionControl = Vector3.MoveTowards(DirectionControl, new Vector3(DirectionControl.x, TargetDirectionControl.z, DirectionControl.z), Time.deltaTime);   
    }

    public bool IsClimbing;

    public void ClimbingLadder()
    {
        IsClimbing = true;
        
    }

/*    public void SetPropertyAction(Vector3 vector3)
    {
        targetMoveToInteractPoint = vector3;      
    }*/
/*    public void PreapreAction(EntityContextAction setAction)
    {
        action = setAction;
        
        IsMoveAction = true;        
    } */
    public void PreapreAction(Vector3 interactPoint)
    {
        targetMoveToInteractPoint = interactPoint;                    
        IsMoveAction = true;        
    }

    private void UpdateDistanceToGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, 1000) == true)
        {
            distanceToGround = Vector3.Distance(transform.position, hit.point);
         }
    }


    [SerializeField] private SpaceSoldier spaceSoldier;
    private float _flyTime;
    private void FallingFromHeight()
    {

        if (IsGrounded == true && _flyTime < _flyFallDead)
        {
            _flyTime = 0;
            return;
        }
        else
            _flyTime += Time.deltaTime;

        if (_flyTime > _flyFallDead)
        { 
            if(IsGrounded == true)
                spaceSoldier?.FallHightDead();
        }

    }
}
