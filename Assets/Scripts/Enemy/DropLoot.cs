using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [SerializeField] private GameObject _lootPref;
    [SerializeField] private float _instHight;

    public void DropingLoot()
    { 
        Instantiate(_lootPref, new Vector3(transform.position.x, transform.position.y + _instHight, transform.position.z), Quaternion.identity);
    }
}
