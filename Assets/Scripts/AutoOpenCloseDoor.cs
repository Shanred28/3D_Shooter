using UnityEngine;

public class AutoOpenCloseDoor : MonoBehaviour
{
    [SerializeField] private Animator _animatorOpenDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out SpaceSoldier spaceSoldier))
        {
            _animatorOpenDoor.CrossFade("Gate_04", 0.45f);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.transform.root.TryGetComponent(out SpaceSoldier spaceSoldier))
        {
            _animatorOpenDoor.CrossFade("Gate_04_close", 0.45f );
        }
    }
}
