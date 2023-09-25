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
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        hint.transform.LookAt(target);

        if(Vector3.Distance(transform.position, target.position) < activeRadius)
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

