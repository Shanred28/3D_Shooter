using System;
using UnityEngine;

[Serializable]
public class ActionUseVehicleProperties : ActionInteractProperties
{
    public Vehicle vehicle;
    public VehicleInputControl vehicleInputControl;
}

public class ActionUseVehicle : ActionInteract
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private ThirdPersonCamera _camera;
    [SerializeField] private CharacterInputController _characterInputController;
    

    [SerializeField] private GameObject _visualModel;

    [SerializeField] private GameObject _hudPlayer;
    [SerializeField] private GameObject _hudVehicle;

    [Header("Setting Fire")]
    [SerializeField] private PlayerShooter _playerShooter;

    public bool isVehicleTurret = false;
    private bool inVehicle;

    private void Start()
    {
        EventOnStart.AddListener(OnActionStarted);
        EventOnEnd.AddListener(OnActionEnded);
    }

    private void Update()
    {
        if (inVehicle == true)
        {
            IsCanEnd = (Properties as ActionUseVehicleProperties).vehicle.LinearVelocity < 2;
        }
    }

    private void OnDestroy()
    {
        EventOnStart.RemoveListener(OnActionStarted);
        EventOnEnd.RemoveListener(OnActionEnded);
    }

    private void OnActionStarted()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;
        inVehicle = true;
        prop.vehicleInputControl.enabled = true;

        // Camera

        prop.vehicleInputControl.AssignCamera(_camera);

        // Vehicle Input
        prop.vehicleInputControl.enabled = true;

        // Character Input
        _characterInputController.enabled = false;

        // Character Movement
        _characterController.enabled = false;
        _characterMovement.enabled = false;

        // Hide Visual Model
        _visualModel.transform.localPosition = _visualModel.transform.localPosition + new Vector3(0,1000,0);

        if (isVehicleTurret == true)
        {
            _playerShooter.enabled = false;
        }

        // HUD
        _hudPlayer.SetActive(false);
        _hudVehicle.SetActive(true);

        prop.vehicle.OnStartDrive();

    }

    private void OnActionEnded()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;
        inVehicle = false;


        prop.vehicleInputControl.enabled = false;

        // Camera

        _characterInputController.AssignCamera(_camera);

        // Vehicle Input
        prop.vehicleInputControl.enabled = false;

        // Character Input
        _characterInputController.enabled = true;

        // Character Movement
        var c = Physics.OverlapCapsule(prop.InteractTransform.localPosition, new Vector3(prop.InteractTransform.localPosition.x, _characterController.height, prop.InteractTransform.localPosition.z), _characterController.radius);
        foreach (var c2 in c)
        {
            if (c2 == null)
                owner.position = prop.InteractTransform.position;
            else
                owner.position = new Vector3(prop.vehicle.transform.localPosition.x + 4, prop.vehicle.transform.localPosition.y, prop.vehicle.transform.localPosition.z);
        }


        _characterController.enabled = true;
        _characterMovement.enabled = true;

        // Show Visual Model
        _visualModel.transform.localPosition = new Vector3(0,0,0);

        if (isVehicleTurret == true)
        {
            _playerShooter.enabled = true;
        }

        // HUD
        _hudPlayer.SetActive(true);
        _hudVehicle.SetActive(false);

        prop.vehicle.OffStartDrive();
      
    }
}
