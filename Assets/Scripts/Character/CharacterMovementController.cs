using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private Vector3 aimingOffset;


    private float dist;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        characterMovement.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        targetCamera.rotateControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (characterMovement.IsAiming == false)
            targetCamera.ScrollDistanceCamera(Input.GetAxis("Mouse ScrollWheel"));

        if (characterMovement.TargetDirectionControl != Vector3.zero || characterMovement.IsAiming == true)
        { 
            targetCamera.IsRotateTarget = true;
        }
        else
            targetCamera.IsRotateTarget = false;

        if (Input.GetMouseButtonDown(1))
        {
            characterMovement.Aiming();
            targetCamera.SetTargetOffset(aimingOffset);
        }

        if (Input.GetMouseButtonUp(1))
        {
            characterMovement.UnAiming();
            targetCamera.SetDefaultOffset();
        }

        if (Input.GetButtonDown("Jump") == true)
        {
            characterMovement.Jump();
        }
           

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            characterMovement.Crouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterMovement.UnCrouch();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) == true)
        {
            characterMovement.Sprint();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) == true)
        {
            characterMovement.UnSprint();
        }
    }
}
