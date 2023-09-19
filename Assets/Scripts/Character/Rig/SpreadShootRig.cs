using UnityEngine;

public class SpreadShootRig : MonoBehaviour
{
    [SerializeField] private UnityEngine.Animations.Rigging.Rig speadShootRig;

    [SerializeField] private float changeWeightLerpRate;

    private float targetWeight;

    private void Update()
    {
        speadShootRig.weight = Mathf.MoveTowards(speadShootRig.weight, targetWeight,Time.deltaTime * changeWeightLerpRate);

        if(speadShootRig.weight == 1)
            targetWeight = 0;
    }

    public void Spread()
    {
        targetWeight = 1f;
    }
}
