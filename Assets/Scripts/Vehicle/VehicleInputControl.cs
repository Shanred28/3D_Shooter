using UnityEngine;

public class VehicleInputControl : MonoBehaviour
{
    [SerializeField] private Vehicle _vehicle;
    [SerializeField] private ThirdPersonCamera _camera;
    [SerializeField] private Vector3 _cameraOffset;

    protected virtual void Start()
    {
        _camera = Player.Instance.MainCamera.GetComponent<ThirdPersonCamera>();
        _camera.transform.SetParent(transform, false);
        AssignCamera(_camera);
        if (_camera != null)
        {
            _camera.IsRotateTarget = false;
            _camera.SetOffset(_cameraOffset);
        }
        
       //_camera.transform.SetParent(transform,false);
       // AssignCamera(_camera);
    }

    protected virtual void Update()
    {
        _vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")));
        _camera.rotateControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public void AssignCamera(ThirdPersonCamera camera)
    {
        _camera = camera;
        _camera.IsRotateTarget = false;
        _camera.SetOffset(_cameraOffset);
        _camera.SetTarget(_vehicle.transform);
        //_camera.transform.LookAt(transform);
    }
}
