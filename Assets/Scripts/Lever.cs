using System.Net.Sockets;
using UnityEngine;

public class Lever : Interactable
{
    public enum LeverState { Down = -1, Middle = 0, Up = 1 }

    [Header("Settings")]
    public LeverState currentState = LeverState.Middle;
    public float rotationAngle = 30f; // inclinación arriba/abajo
    public LightsPuzzle puzzleManager;

    private Quaternion baseRotation;

    private void Start()
    {
        baseRotation = transform.localRotation;
        ApplyRotation();
    }

    public override void Interact()
    {
        if (currentState == LeverState.Middle)
            currentState = LeverState.Up;
        else if (currentState == LeverState.Up)
            currentState = LeverState.Down;
        else
            currentState = LeverState.Middle;

        ApplyRotation();
        puzzleManager?.CheckSolution();
    }

    private void ApplyRotation()
    {
        float angle = 0f;

        if (currentState == LeverState.Up)
            angle = rotationAngle;
        else if (currentState == LeverState.Down)
            angle = -rotationAngle;

        transform.localRotation = baseRotation * Quaternion.Euler(angle, 0f, 0f);
    }

    public override void ShowMess()
    {
        InteractMessageScript.Instance.ShowShortMessage("Pulsa E para mover el interruptor");
    }

    public int GetValue()
    {
        return (int)currentState;
    }
}