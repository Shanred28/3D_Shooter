using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    [SerializeField] float activeRadius;
    
    private Canvas canvas;
    private Transform target;
    private Transform lookRotation;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        lookRotation = Camera.main.transform;
        target = Player.Instance.transform;
    }

    private void Update()
    {
        if (lookRotation == null)
        {
            lookRotation = Camera.main.transform;
            if (lookRotation == null) return;
        }
        hint.transform.LookAt(lookRotation);

        if(Vector3.Distance(transform.position, Player.Instance.transform.position) < activeRadius)
            hint.SetActive(true);
        else
            hint.SetActive(false);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activeRadius);
    }
#endif
}

