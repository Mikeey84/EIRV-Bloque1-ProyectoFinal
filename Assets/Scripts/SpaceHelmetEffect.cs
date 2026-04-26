using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpaceHelmetEffect : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Volume globalVolume;

    [Header("Configuración del Efecto")]
    [Range(0f, 1f)]
    public float vignetteIntensity = 0.699f;
    public Color vignetteTint = new Color(1f, 0.5896226f, 0.5896226f); // Color del viñeteado original

    [Range(-100f, 100f)]
    public float whiteBalanceTemperature = -30f;

    [Range(0f, 1f)]
    public float chromaticAberrationIntensity = 0.15f;

    [Range(0f, 2f)]
    public float bloomIntensity = 0.3f;
    public float bloomThreshold = 0.9f;

    [Range(-1f, 1f)]
    public float lensDistortion = -0.2f;

    public Color colorAdjustmentTint = new Color(0.85f, 0.95f, 1.1f);

    [Header("Oscurecimiento Global")]
    [Range(-100f, 0f)]
    public float exposureAdjustment = -1.5f; // Oscurecer la escena para resaltar el sol

    private void Start()
    {
        if (globalVolume == null)
        {
            globalVolume = FindObjectOfType<Volume>();
            if (globalVolume == null)
            {
                Debug.LogError("No se encontró ningún Global Volume en la escena");
            }
        }
    }

    public void EnableSpaceHelmetEffect()
    {
        if (globalVolume == null || globalVolume.profile == null) return;

        VolumeProfile profile = globalVolume.profile;

        // VIGNETTE - Usando los valores exactos que pasaste
        if (!profile.TryGet(out Vignette vignette))
        {
            vignette = profile.Add<Vignette>(false);
        }
        vignette.active = true;
        vignette.color.Override(vignetteTint);
        vignette.center.Override(new Vector2(0.5f, 0.5f));
        vignette.intensity.Override(vignetteIntensity);
        vignette.smoothness.Override(0.394f);
        vignette.rounded.Override(false);

        // WHITE BALANCE - Tonos fríos azulados del espacio
        if (!profile.TryGet(out WhiteBalance whiteBalance))
        {
            whiteBalance = profile.Add<WhiteBalance>(false);
        }
        whiteBalance.active = true;
        whiteBalance.temperature.Override(whiteBalanceTemperature);
        whiteBalance.tint.Override(5f);

        // CHROMATIC ABERRATION - Distorsión de lente del visor
        if (!profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration = profile.Add<ChromaticAberration>(false);
        }
        chromaticAberration.active = true;
        chromaticAberration.intensity.Override(chromaticAberrationIntensity);

        // BLOOM - Brillo de estrellas y luces intensas
        if (!profile.TryGet(out Bloom bloom))
        {
            bloom = profile.Add<Bloom>(false);
        }
        bloom.active = true;
        bloom.intensity.Override(bloomIntensity);
        bloom.threshold.Override(bloomThreshold);
        bloom.scatter.Override(0.7f);
        bloom.tint.Override(new Color(0.9f, 0.95f, 1f));

        // LENS DISTORTION - Curvatura del visor del casco
        if (!profile.TryGet(out LensDistortion lensDistortion))
        {
            lensDistortion = profile.Add<LensDistortion>(false);
        }
        lensDistortion.active = true;
        lensDistortion.intensity.Override(this.lensDistortion);
        lensDistortion.scale.Override(1f);

        // COLOR ADJUSTMENTS - Tinte azulado general + OSCURECIMIENTO
        if (!profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments = profile.Add<ColorAdjustments>(false);
        }
        colorAdjustments.active = true;
        colorAdjustments.colorFilter.Override(colorAdjustmentTint);
        colorAdjustments.contrast.Override(5f);
        colorAdjustments.saturation.Override(-10f);
        colorAdjustments.postExposure.Override(exposureAdjustment); // OSCURECER para que el sol destaque

        // TONEMAPPING - Para mejor rango dinámico en el espacio
        if (!profile.TryGet(out Tonemapping tonemapping))
        {
            tonemapping = profile.Add<Tonemapping>(false);
        }
        tonemapping.active = true;
        tonemapping.mode.Override(TonemappingMode.ACES);

        Debug.Log("Efecto de casco espacial activado");
    }

    public void DisableSpaceHelmetEffect()
    {
        if (globalVolume == null || globalVolume.profile == null) return;

        VolumeProfile profile = globalVolume.profile;

        if (profile.TryGet(out Vignette vignette))
            vignette.active = false;

        if (profile.TryGet(out WhiteBalance whiteBalance))
            whiteBalance.active = false;

        if (profile.TryGet(out ChromaticAberration chromaticAberration))
            chromaticAberration.active = false;

        if (profile.TryGet(out Bloom bloom))
            bloom.active = false;

        if (profile.TryGet(out LensDistortion lensDistortion))
            lensDistortion.active = false;

        if (profile.TryGet(out ColorAdjustments colorAdjustments))
            colorAdjustments.active = false;

        if (profile.TryGet(out Tonemapping tonemapping))
            tonemapping.active = false;

        Debug.Log("Efecto de casco espacial desactivado");
    }

    [ContextMenu("Activar Efecto")]
    private void TestEnable()
    {
        EnableSpaceHelmetEffect();
    }

    [ContextMenu("Desactivar Efecto")]
    private void TestDisable()
    {
        DisableSpaceHelmetEffect();
    }
}