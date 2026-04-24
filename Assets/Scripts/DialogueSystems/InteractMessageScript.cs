using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractMessageScript : MonoBehaviour
{
    public static InteractMessageScript Instance { get; private set; }

    [SerializeField] private CanvasGroup interactMessageCanvasGroup;
    [SerializeField] private TextMeshProUGUI interatMessageText;
    [SerializeField] private float displayTime = 5f;
    [SerializeField] private float typewriterSpeed = 0.05f;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> interactSounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        interactMessageCanvasGroup.alpha = 0;
        audioSource = GetComponent<AudioSource>();
    }

    public void ShowMessage(string message, float displayTime = 5f)
    {
        interatMessageText.text = message;
        StopAllCoroutines();
        StartCoroutine(ShowInfoMessage());
    }

    public void ShowMessage(string message)
    {
        interatMessageText.text = message;
        StopAllCoroutines();
        StartCoroutine(ShowInfoMessage());
    }


    IEnumerator ShowInfoMessage()
    {
        float duration = 0;
        interactMessageCanvasGroup.alpha = 0;
        interactMessageCanvasGroup.gameObject.SetActive(true);
        while (duration <= 1)
        {
            duration += Time.deltaTime / (displayTime / 2);
            interactMessageCanvasGroup.alpha = duration;
            yield return null;
        }
        while (duration >= 0)
        {
            duration -= Time.deltaTime / (displayTime / 2);
            interactMessageCanvasGroup.alpha = duration;
            yield return null;
        }
        interatMessageText.text = "";
        interactMessageCanvasGroup.gameObject.SetActive(false);
    }

    public void ShowMessageTypewriter(string message)
    {
        interatMessageText.text = message;
        StopAllCoroutines();
        StartCoroutine(ShowInfoMessageTypewriter());
    }

    IEnumerator ShowInfoMessageTypewriter()
    {
        string fullText = interatMessageText.text;
        interatMessageText.text = "";
        interactMessageCanvasGroup.alpha = 1;
        interactMessageCanvasGroup.gameObject.SetActive(true);

        foreach (char letter in fullText)
        {
            interatMessageText.text += letter;
            audioSource.PlayOneShot(interactSounds[Random.Range(0, interactSounds.Count)]);
            yield return new WaitForSeconds(typewriterSpeed);
        }

        yield return new WaitForSeconds(displayTime);

        float duration = 1f;
        while (duration >= 0)
        {
            duration -= Time.deltaTime / (displayTime / 2);
            interactMessageCanvasGroup.alpha = duration;
            yield return null;
        }
        interatMessageText.text = "";
        interactMessageCanvasGroup.gameObject.SetActive(false);
    }
}