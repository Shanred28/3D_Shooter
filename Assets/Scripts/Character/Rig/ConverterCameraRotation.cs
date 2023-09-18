using UnityEngine;

public class ConverterCameraRotation : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform cameraLook;
    [SerializeField] private Vector3 lookOffset;

    [SerializeField] private float topAngleLimit;
    [SerializeField] private float bottomAngleLimit;

    void Update()
    {
        Vector3 Angles = new Vector3(0, 0, 0);

        Angles.z = camera.eulerAngles.x;

        if (Angles.z >= topAngleLimit || Angles.z <= bottomAngleLimit)
        {
            transform.LookAt(cameraLook.position + lookOffset);

            Angles.x = transform.eulerAngles.x;
            Angles.y = transform.eulerAngles.y;

            transform.eulerAngles = Angles;
        }
    }


}
