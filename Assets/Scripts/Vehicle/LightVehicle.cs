using System.Collections.Generic;
using UnityEngine;

public class LightVehicle : MonoBehaviour
{
    private List<Light> lights = new List<Light>();

    private void Start()
    {
        AddLight();
    }

    private void AddLight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
            {
                Light light = transform.GetChild(i).GetComponent<Light>();
                if (light != null)
                {
                    lights.Add(light);
                    light.enabled = false;
                }
            }
        }
    }

    public void OnLight()
    { 
        foreach (Light light in lights) 
        {
            light.enabled = true;
        }
    }

    public void OffLight()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }
}
