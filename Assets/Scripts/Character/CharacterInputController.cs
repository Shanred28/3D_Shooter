using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;

    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private Vector3 aimingOffset;
    [SerializeField] private EntityActionCollector targetActionCollector;


    private float dist;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
/*        if (prepare == false)
        {*/
            characterMovement.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            targetCamera.rotateControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        /*  }

         else
         {
             //characterMovement.TargetDirectionControl = Vector3.MoveTowards(transform.position, targetMoveToInteractPoint, Time.deltaTime * 0.0013f);
             var dis = Vector3.Distance(transform.position, targetMoveToInteractPoint);
             characterMovement.TargetDirectionControl = Vector3.MoveTowards(transform.position, targetMoveToInteractPoint, dis);
             transform.LookAt(targetMoveToInteractPoint);
             if (dis < 1)
             {
                 characterMovement.TargetDirectionControl = transform.position;
                 prepare = false;
             }


         }*/
        targetCamera.IsRotateTarget = true;
        if (characterMovement.IsAiming == false)
            targetCamera.ScrollDistanceCamera(Input.GetAxis("Mouse ScrollWheel"));

/*        if (characterMovement.TargetDirectionControl != Vector3.zero || characterMovement.IsAiming == true)
        { 
            
        }
        else
            targetCamera.IsRotateTarget = false;*/

        if (Input.GetKeyDown(KeyCode.E))
        {

            characterMovement.PreapreAction();

 
        }

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
}
