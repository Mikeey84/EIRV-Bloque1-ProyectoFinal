using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RotateOnInteract : Interactable
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationDuration = 0.5f;

    [Header("Events")]
    public UnityEvent onInteract;          // se lanza al interactuar
    public UnityEvent onRotationComplete;  // se lanza al terminar la rotación
    public bool hasRotated = false;
    private bool isRotating = false;

    public override void Interact()
    {
        if (isRotating || hasRotated) return;
        hasRotated = true;
        onInteract?.Invoke();
        StartCoroutine(RotateSmoothly());
    }

    private IEnumerator RotateSmoothly()
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(rotationAxis * 90f);

        float time = 0f;

        while (time < rotationDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / rotationDuration);

            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;

        onRotationComplete?.Invoke();
    }

    public override void ShowMess()
    {
        Debug.Log("Pulsa para rotar suavemente");
    }
}