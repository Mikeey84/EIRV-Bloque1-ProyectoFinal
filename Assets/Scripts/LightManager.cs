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
        ChangeColors(Color.red, MaxIntensity);
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

    public void ChangeColors(Color color, float intensity)
    {
        foreach (var light in lights)
        {
            light.color = color;
            light.intensity = intensity;
        }
    }
}