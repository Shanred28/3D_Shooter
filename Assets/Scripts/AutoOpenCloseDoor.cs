using System;
using UnityEngine;

[Serializable]
public class AnimationName
{
    public string Open;
    public string Close;
}

[RequireComponent(typeof(BoxCollider))]
public class AutoOpenCloseDoor : MonoBehaviour
{
    [SerializeField] private Animator _animatorOpenDoor;
    [SerializeField] private AnimationName animationName; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out SpaceSoldier spaceSoldier))
        {
            _animatorOpenDoor.CrossFade(animationName.Open, 0.45f);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.transform.root.TryGetComponent(out SpaceSoldier spaceSoldier))
        {
            _animatorOpenDoor.CrossFade(animationName.Close, 0.45f );
        }
    }
}
