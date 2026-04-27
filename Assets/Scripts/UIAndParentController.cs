using System.Collections;
using UnityEngine;

public class UIAndParentController : MonoBehaviour
{
    [Header("Canvas")]
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    [Header("Player & Parent")]
    public Transform player;
    public Transform originalParent;
    public Transform newParent;

    [Header("Audio Sources")]
    public AudioSource breath;
    public AudioSource heartbeat;
    public AudioSource tinnitus;

    [Range(0f, 1f)]
    public float loweredVolume = 0f;

    private float originalVolume1;
    private float originalVolume2;
    private float originalVolume3;

    private Coroutine fadeCoroutine;
    private Coroutine volumeCoroutine;

    private void Start()
    {
        if (breath != null) originalVolume1 = breath.volume;
        if (heartbeat != null) originalVolume2 = heartbeat.volume;
        if (tinnitus != null) originalVolume3 = tinnitus.volume;
    }

    public void ActivateCanvasAndReparent()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (volumeCoroutine != null) StopCoroutine(volumeCoroutine);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        SetAudioVolumesInstant(loweredVolume);

        if (player != null && newParent != null)
        {
            player.SetParent(newParent);
        }
    }

    public void DeactivateCanvas()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (volumeCoroutine != null) StopCoroutine(volumeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutCanvas());
        volumeCoroutine = StartCoroutine(FadeAudioBackIn());
    }

    private IEnumerator FadeOutCanvas()
    {
        if (canvasGroup == null) yield break;

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeAudioBackIn()
    {
        float elapsed = 0f;

        float startVolume1 = breath != null ? breath.volume : 0f;
        float startVolume2 = heartbeat != null ? heartbeat.volume : 0f;
        float startVolume3 = tinnitus != null ? tinnitus.volume : 0f;

        tinnitus.Play();


        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            if (breath != null)
                breath.volume = Mathf.Lerp(startVolume1, originalVolume1, t);

            if (heartbeat != null)
                heartbeat.volume = Mathf.Lerp(startVolume2, originalVolume2, t);

            if (tinnitus != null)
                tinnitus.volume = Mathf.Lerp(startVolume3, originalVolume3, t);

            yield return null;
        }

        if (breath != null) breath.volume = originalVolume1;
        if (heartbeat != null) heartbeat.volume = originalVolume2;
        if (tinnitus != null) tinnitus.volume = originalVolume3;
    }

    private void SetAudioVolumesInstant(float volume)
    {
        if (breath != null) breath.volume = volume;
        if (heartbeat != null) heartbeat.volume = volume;
        if (tinnitus != null) tinnitus.volume = volume;
    }
}