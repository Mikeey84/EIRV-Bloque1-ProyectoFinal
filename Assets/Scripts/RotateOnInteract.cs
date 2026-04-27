using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RotateOnInteract : Interactable
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.forward; // Para rotar en Z
    public float rotationAngle = 90f;
    public float rotationDuration = 0.5f;

    [Header("Messages")]
    public string interactMessage = "Presiona E para rotar";

    [Header("Events")]
    public UnityEvent onInteract;
    public UnityEvent onRotationComplete;

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
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);

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
        if (!hasRotated && !isRotating)
            InteractMessageScript.Instance.ShowShortMessage(interactMessage);
    }
}