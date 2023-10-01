using UnityEngine;
using UnityEngine.UI;

public class UISight : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private Image imageSight;

    private void Update()
    {
        imageSight.enabled = characterMovement.IsAiming;
    }

}
