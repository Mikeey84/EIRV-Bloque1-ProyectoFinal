using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private List<Light> lights;


    private void Start()
    {
        foreach (var light in lights)
        {
            ChangeColors(Color.red, 1f);
        }
    }
    public void SetLights(bool state)
    {
        foreach (var light in lights)
        {
            light.enabled = state;
        }
    }

    public void ChangeColors(Color color, float intensity)
    {
        foreach (var light in lights)
        {
            light.color = color;
            light.intensity = intensity;
        }
    }
}   