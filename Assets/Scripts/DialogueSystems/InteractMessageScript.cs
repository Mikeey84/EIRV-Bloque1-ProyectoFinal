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

    private Coroutine routine = null;

    private float _shortMessageTimer = 0f;
    private bool _shortMessageActive = false;
    private const float ShortMessageTimeout = 0.1f; 

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

    private void Update()
    {
        if (!_shortMessageActive) return;

        _shortMessageTimer -= Time.deltaTime;

        if (_shortMessageTimer <= 0f)
        {
            _shortMessageActive = false;
            interactMessageCanvasGroup.alpha = 0;
            interactMessageCanvasGroup.gameObject.SetActive(false);
            interatMessageText.text = "";
        }
    }

    public void ShowShortMessage(string message)
    {
        _shortMessageTimer = ShortMessageTimeout;

        if (!_shortMessageActive)
        {
            _shortMessageActive = true;
            StopAllCoroutines();
            routine = null;
            interatMessageText.text = message;
            interactMessageCanvasGroup.alpha = 1;
            interactMessageCanvasGroup.gameObject.SetActive(true);
        }
        else
        {
            interatMessageText.text = message;
        }
    }

    public void Hide()
    {
        interatMessageText.text = "";
        interactMessageCanvasGroup.gameObject.SetActive(false);
        _shortMessageTimer = 0;
        _shortMessageActive = false;
    }

    public void ShowShortMessage(string message, float timeout = ShortMessageTimeout)
    {
        _shortMessageTimer = timeout;

        if (!_shortMessageActive)
        {
            _shortMessageActive = true;
            StopAllCoroutines();
            routine = null;
            interatMessageText.text = message;
            interactMessageCanvasGroup.alpha = 1;
            interactMessageCanvasGroup.gameObject.SetActive(true);
        }
        else
        {
            interatMessageText.text = message;
        }
    }

    public void ShowMessage(string message, float displayTime = 5f)
    {
        _shortMessageActive = false;
        interatMessageText.text = message;
        StopAllCoroutines();
        routine = StartCoroutine(ShowInfoMessage());
    }

    public void ShowMessage(string message)
    {
        _shortMessageActive = false;
        interatMessageText.text = message;
        StopAllCoroutines();
        routine = StartCoroutine(ShowInfoMessage());
    }

    public void ShowMessageTypewriter(string message)
    {
        _shortMessageActive = false;
        interatMessageText.text = message;
        StopAllCoroutines();
        StartCoroutine(ShowInfoMessageTypewriter());
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
        routine = null;
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