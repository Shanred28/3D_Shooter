using UnityEngine;

public class Rotete : MonoBehaviour
{
    [SerializeField] private  float _speedRotate;
    private void Update()
    {
        transform.Rotate(0, _speedRotate * Time.deltaTime, 0);
    }

}
