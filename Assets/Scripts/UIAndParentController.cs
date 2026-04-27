using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAndParentController : MonoBehaviour
{
    [Header("Canvas")]
    public CanvasGroup canvasGroup;

    [Header("Player & Parent")]
    public Transform player;
    public Transform originalParent;
    public Transform newParent;

    public void ActivateCanvasAndReparent()
    {
        // Poner alpha a 1 (visible)
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        // Hacer hijo al jugador
        if (player != null && newParent != null)
        {
            player.SetParent(newParent);
        }
    }

    public void DeactivateCanvas()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeOutCanvas());
        }
    }

    private IEnumerator FadeOutCanvas()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}