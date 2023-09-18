using UnityEngine;

/// <summary>
/// Base class of all interactive game objects on scene.
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Names of object for the user.
    /// </summary>
    [SerializeField] private string m_Nicname;
    public string Nicname => m_Nicname;
}


