using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;

    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private Vector3 aimingOffset;

    [SerializeField] private Vector3 _defaultOffset;


    public bool IsActionEveleble =false;
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


        if (Input.GetMouseButton(0))
        {
            if(characterMovement.IsAiming == true)
               playerShooter.Shoot();
        }
            

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

    public void AssignCamera(ThirdPersonCamera camera)
    {
        targetCamera = camera;
        targetCamera.IsRotateTarget = false;
        targetCamera.SetOffset(_defaultOffset);
        targetCamera.SetTarget(characterMovement.transform);
    }
}
