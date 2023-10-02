using System;
using UnityEngine;

[Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider _leftWheelCollider;
    [SerializeField] private WheelCollider _rightWheelCollider;
    [SerializeField] private Transform _leftWheelMesh;
    [SerializeField] private Transform _rightWheelMesh;
    [SerializeField] private bool _motor;
    public bool Motor => _motor;

    [SerializeField] private bool _steering;
    public bool Steering => _steering;

    // Public

    public void SetTorgue(float torque)
    {
        if (_motor == false) return;

        _leftWheelCollider.motorTorque = torque;
        _rightWheelCollider.motorTorque = torque;
    }

    public void Break(float breakTorque)
    {
        _leftWheelCollider.brakeTorque = breakTorque;
        _rightWheelCollider.brakeTorque = breakTorque;
    }

    public void SetSteerAngle(float angle)
    { 
        if(_steering == false) return;

        _leftWheelCollider.steerAngle = angle;
        _rightWheelCollider.steerAngle = angle;
    }

    public void UpdateMeshTransform()
    {
        UpdateWheelTransform(_leftWheelCollider, ref _leftWheelMesh);
        UpdateWheelTransform(_rightWheelCollider, ref _rightWheelMesh);

    }

    // Private

    private void UpdateWheelTransform(WheelCollider wheelCollider, ref Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class WheeledVehicle : Vehicle
{
    [SerializeField] private WheelAxle[] _wheelAxles;

    [SerializeField] private float _maxMotorTorque;
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _breakTorque;

    private Rigidbody _rb;
    public override float LinearVelocity => _rb.velocity.magnitude;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float targetMotor = _maxMotorTorque * TargetInputControl.z; 
        float breakTorque = _breakTorque * TargetInputControl.y; 
        float steering = _maxSteerAngle * TargetInputControl.x; 


        for (int i = 0; i < _wheelAxles.Length; i++)
        {
            if (breakTorque == 0 && LinearVelocity < _maxSpeed)
            {
                _wheelAxles[i].Break(0);
                _wheelAxles[i].SetTorgue(targetMotor);
            }

            if (LinearVelocity > _maxSpeed)
            {
                _wheelAxles[i].Break(breakTorque * 0.2f);
            }
            else
            {
                _wheelAxles[i].Break(breakTorque);
            }

            _wheelAxles[i].Break(breakTorque);


            _wheelAxles[i].SetSteerAngle(steering);

            _wheelAxles[i].UpdateMeshTransform();
        }
    }
}
