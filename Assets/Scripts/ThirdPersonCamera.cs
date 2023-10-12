using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float distanceCamera;
    [SerializeField] private float minDistanceCamera;
    [SerializeField] private float maxDistanceCamera;


    [SerializeField] private float sensetive;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float offsetLerpRate;
    [SerializeField] private float rotateTargetLerpRate;

    [Header("RotationLimit")]
    [SerializeField] private float maxAngleLimitY;
    [SerializeField] private float minAngleLimitY;


    [SerializeField] private float minDistance;
    [SerializeField] private float distanceLerpRate; 
    [SerializeField] private float distanceOffsetFromCollisionHit; 

    [HideInInspector] public bool IsRotateTarget;
    [HideInInspector] public Vector2 rotateControl;

    private float deltaRotationX;
    private float deltaRotationY;

    private float currentDistance;

    private float defaultDistanceCamera;
    private float playerDistanceCamera;
    private Vector3 targetOffset;
    private Vector3 defaultOffset;

    private void Start()
    {
        defaultOffset = _offset;
        targetOffset = _offset;
        defaultDistanceCamera = distanceCamera;
    }
    private void Update()
    {
        deltaRotationX += rotateControl.x * sensetive;
        deltaRotationY += rotateControl.y * sensetive;

        deltaRotationY = ClampAngle(deltaRotationY, minAngleLimitY, maxAngleLimitY);

        // offset = Vector3.Lerp(offset, targetOffset, Time.deltaTime * sensetive);
        _offset = Vector3.MoveTowards(_offset, targetOffset, Time.deltaTime * offsetLerpRate);
        
        Quaternion finalRotation = Quaternion.Euler(-deltaRotationY, deltaRotationX, 0);
        Vector3 finalPosition = _target.position - (finalRotation * Vector3.forward * distanceCamera);
        finalPosition = AddLocalOffset(finalPosition);

        //Calculate current distance
        float targetDistance = distanceCamera;
        RaycastHit hit;

        if (Physics.Linecast(_target.position + new Vector3(0, _offset.y, 0), finalPosition, out hit) == true)
        { 
            float distanceToHit = Vector3.Distance(_target.position + new Vector3(0,_offset.y,0), hit.point);

            if (hit.transform != _target)
            {
                if (distanceToHit < distanceCamera)
                    targetDistance = distanceToHit - distanceOffsetFromCollisionHit;
            }
        }

        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, Time.deltaTime * distanceLerpRate);
        currentDistance = Mathf.Clamp(currentDistance,minDistance,distanceCamera);

        //Correct camera position
        finalPosition = _target.position - (finalRotation * Vector3.forward * currentDistance);
        
        //Apply transform
        transform.rotation = finalRotation;
        transform.position = finalPosition;
        transform.position = AddLocalOffset(finalPosition);

        if (IsRotateTarget == true)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            _target.rotation = Quaternion.RotateTowards(_target.rotation, targetRotation, Time.deltaTime * rotateTargetLerpRate);
        }
    }

    public void ScrollDistanceCamera(float distanse)
    {
        distanceCamera = Mathf.Clamp(distanceCamera + distanse * sensetive, minDistanceCamera, maxDistanceCamera);
    }

    private Vector3 AddLocalOffset(Vector3 position)
    {
        Vector3 result = position;
        result += new Vector3(0, _offset.y, 0);
        result += transform.right * _offset.x;
        result += transform.forward * _offset.z;

        return result;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    public void SetTargetOffset(Vector3 offset)
    {
        targetOffset = offset;
        playerDistanceCamera = distanceCamera;
        distanceCamera = defaultDistanceCamera;
        //distanceCamera = Mathf.Lerp(distanceCamera, targetOffset.y, Time.deltaTime * sensetive);


    }

    public void SetOffset(Vector3 offset)
    {
        _offset = offset;
        targetOffset = offset;
    }

    public void SetDefaultOffset()
    {
        targetOffset = defaultOffset;
         distanceCamera = playerDistanceCamera;
       // distanceCamera = Mathf.Lerp(distanceCamera, defaultDistanceCamera, Time.deltaTime * sensetive);
    }

    public void SetTarget(Transform target)
    { 
      _target = target;
    }
}
