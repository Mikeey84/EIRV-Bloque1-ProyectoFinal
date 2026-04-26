using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class InteractableEvents : Interactable
{
    [Header("Mensajes")]
    public string mensajeAlMirar = "Presiona E para interactuar";

    [Header("Eventos")]
    public List<UnityEvent> onInteractEvents = new List<UnityEvent>();

    public override void ShowMess()
    {
        InteractMessageScript.Instance.ShowShortMessage(mensajeAlMirar);
    }

    public override void Interact()
    {
        foreach (UnityEvent evento in onInteractEvents)
        {
            evento?.Invoke();
        }

        this.gameObject.layer = 0;
    }
}