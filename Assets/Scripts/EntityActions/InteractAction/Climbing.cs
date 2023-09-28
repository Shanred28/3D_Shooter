using UnityEngine;

public class Climbing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out CharacterMovement characterMovement))
        {
            characterMovement.IsClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out CharacterMovement characterMovement))
        {
            characterMovement.IsClimbing = false;
        }
    }
}
