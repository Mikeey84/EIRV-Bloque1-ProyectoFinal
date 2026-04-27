using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private List<Light> lights;

    [Header("Parpadeo")]
    public bool Parpadeo = true;
    public float BlinkSpeed = 2f;
    public float MinIntensity = 0f;
    public float MaxIntensity = 1f;

    private void Start()
    {
        ChangeColors(new Vector3(1,0,0), MaxIntensity);
    }

    private void Update()
    {
        if (Parpadeo)
        {
            float t = Mathf.PingPong(Time.time * BlinkSpeed, 1f);
            float intensity = Mathf.Lerp(MinIntensity, MaxIntensity, t);

            foreach (var light in lights)
            {
                light.intensity = intensity;
                light.enabled = intensity > 0.01f;
            }
        }
    }

    public void SetLights(bool state)
    {
        foreach (var light in lights)
        {
            light.enabled = state;
        }
    }

    public void SetIntensity(float inte)
    {
        foreach (var light in lights)
        {
            light.intensity = inte;
        }
    }

    public void SetParpadeo(bool state)
    {
        Parpadeo = state;
    }

    public void ChangeColors(int color)
    {
        Color colorAux;
        switch (color)
        {
            case 0: colorAux = Color.red; break;
            case 1: colorAux = Color.green; break;
            case 2: colorAux = Color.blue; break;
            default: colorAux = Color.white; break;
        }
        foreach (var light in lights)
        {
            light.color = colorAux;
        }
    }

    public void ChangeColors(Vector3 color, float intensity)
    {
        foreach (var light in lights)
        {
            light.color = new Color(color.x, color.y, color.z);
            light.intensity = intensity;
        }
    }
}