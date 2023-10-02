using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverVehicle : Vehicle
{
    [SerializeField] private float _thrustForward;
    [SerializeField] private float _thrustTorque;

    [SerializeField] private float _dragLinear;
    [SerializeField] private float _dragAngular;

    [SerializeField] private float _hoverHeight;

    [SerializeField] private float _hoverForce;

    [SerializeField] private Transform[] _hoverJets;
    

    private Rigidbody rb;
    private bool isGrounded;
    public override float LinearVelocity => rb.velocity.magnitude;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        ComputeForces();
    }

    private void ComputeForces()
    {
        isGrounded = false;

        for (int i = 0; i < _hoverJets.Length; i++)
        {
            if (ApplyJetForce(_hoverJets[i]) == true)
            {
                isGrounded = true;
            }
        }

        if (isGrounded == true)
        {
            rb.AddRelativeForce(Vector3.forward * _thrustForward * TargetInputControl.z);
            rb.AddRelativeTorque(Vector3.up * _thrustTorque * TargetInputControl.x);
        }

        // Linear drag

        float dragCoeff = _thrustForward  / _maxSpeed;
        Vector3 dragForce = rb.velocity * -dragCoeff;

        if (isGrounded == true)
        {
            rb.AddForce(dragForce,ForceMode.Acceleration);
        }

        // Angular drag

        Vector3 dragAngularForce = -rb.angularVelocity * _dragAngular;
        rb.AddTorque(dragAngularForce, ForceMode.Acceleration);
    }

    public bool ApplyJetForce(Transform tr)
    { 
        Ray ray = new Ray(tr.position, - tr.up);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _hoverHeight) == true)
        {
            float s = (_hoverHeight - hit.distance / _hoverHeight);

            Vector3 force = (s * _hoverForce) * hit.normal;

            rb.AddForceAtPosition(force, tr.position, ForceMode.Acceleration);
            return true;
        }
        return false;
    }
}
