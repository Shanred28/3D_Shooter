using UnityEngine;

public class AimingRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private UnityEngine.Animations.Rigging.Rig[] rigs;

    [SerializeField] private float changeWeightLerpRate;

    private float targetWeight;

    private void Update()
    {
        for (int i = 0; i < rigs.Length; i++)
        {
            rigs[i].weight = Mathf.MoveTowards(rigs[i].weight, targetWeight, Time.deltaTime * changeWeightLerpRate);
        }
        if (characterMovement.IsAiming == true)
            targetWeight = 1f;
        else 
            targetWeight = 0f;
    }
}
