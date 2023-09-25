using UnityEngine;

public class Drone : Destructible
{
    [Header("Main")]
    [SerializeField] private Transform mainMesh;
    [SerializeField] private Weapon[] weaponTurret;

    [Header("View")]
    [SerializeField] private GameObject[] meshComponents;

    [SerializeField] private Renderer[] mashRenderers;
    [SerializeField] private Material[] materialsDead;

    [Header("Movement")]
    [SerializeField] private float hoverAmplitude;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationLerpRate;

    public Transform MainMesh => mainMesh;
    private void Update()
    {
        Hover();
    }

    protected override void OnDeath()
    {
        if (IsDestroy == false)
        {
            EventOnDeath?.Invoke();
            enabled = false;

            for (int i = 0; i < meshComponents.Length; i++)
            {
                if (meshComponents[i].GetComponent<Rigidbody>() == null)
                    meshComponents[i].AddComponent<Rigidbody>();
            }

            for (int i = 0; i < mashRenderers.Length; i++)
            {
                mashRenderers[i].material = materialsDead[i];
            }
                IsDestroy = true;
        }
    }

    private void Hover()
    {
        mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime, 0);
    }

    //Public API

    public void LookAt(Vector3 target)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), Time.deltaTime * rotationLerpRate);
    }

    public void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
    }

    public void Fire(Vector3 target)
    {
        for (int i = 0; i < weaponTurret.Length; i++)
        {
            weaponTurret[i].FirePointLookAt(target);
            weaponTurret[i].Fire();
        }
    }
}
