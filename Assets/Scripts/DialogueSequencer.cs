using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueSequencer : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueStep
    {
        [TextArea(3, 10)]
        public string text;
        public float delayAfterFinished; // Tiempo extra de espera tras terminar de escribir
    }

    [Header("Configuración de Textos")]
    public List<DialogueStep> sequence;

    [Header("Evento Final")]
    public UnityEvent onSequenceFinished;
    public GameObject objectToActivate;

    public void StartSequence()
    {
        StartCoroutine(DialogueRoutine());
    }

    private IEnumerator DialogueRoutine()
    {
        foreach (var step in sequence)
        {
            // Llamamos a tu método existente
            InteractMessageScript.Instance.ShowMessageTypewriter(step.text);

            // Calculamos cuánto tiempo esperar: 
            // (Letras * velocidad) + tiempo de lectura (displayTime) + delay extra
            // Usamos un cálculo basado en tus variables de InteractMessageScript
            float typewriterDuration = step.text.Length * 0.05f; // 0.05f es tu speed por defecto
            float waitTime = typewriterDuration + 5f + step.delayAfterFinished; // 5f es tu displayTime

            yield return new WaitForSeconds(waitTime);
        }

        // Al terminar todos los textos
        onSequenceFinished?.Invoke();

        if (objectToActivate != null)
            objectToActivate.SetActive(true);
    }
}