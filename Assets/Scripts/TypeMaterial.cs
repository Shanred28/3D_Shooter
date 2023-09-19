using UnityEngine;

public enum materialType
{
    Stone,
    Metall

}
public class TypeMaterial : MonoBehaviour
{


    [SerializeField] private materialType material;
    public materialType Material => material;

}
