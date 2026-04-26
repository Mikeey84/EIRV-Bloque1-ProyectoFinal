using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Door : Interactable
{
    [Header("Door")]
    public string OpenAnimation;
    public string CloseAnimation;
    public string OpenMessage;
    public string CloseMessage;
    public string LockedMessage;
    public bool locked;

    [Header("Events")]
    public List<UnityEvent> onOpenEvents = new List<UnityEvent>();
    public List<UnityEvent> onCloseEvents = new List<UnityEvent>();

    private Animator _animator;
    private bool _isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        if (locked)
        {
            InteractMessageScript.Instance.ShowShortMessage(LockedMessage, 1.5f);
            return;
        }

        Debug.Log(_isOpen ? "Closing door" : "Opening door");
        _isOpen = !_isOpen;
        _animator.Play(_isOpen ? OpenAnimation : CloseAnimation);

        // Ejecutar eventos correspondientes
        if (_isOpen)
        {
            foreach (UnityEvent evento in onOpenEvents)
            {
                evento?.Invoke();
            }
        }
        else
        {
            foreach (UnityEvent evento in onCloseEvents)
            {
                evento?.Invoke();
            }
        }
    }

    public override void ShowMess()
    {
        if (!locked)
            InteractMessageScript.Instance.ShowShortMessage(!_isOpen ? OpenMessage : CloseMessage);
    }

    public void SetAnimation(string Animation)
    {
        if (_animator != null)
            _animator.Play(Animation);
    }

    public void SetState(bool l)
    {
        locked = l;
    }

    public void CloseDoor()
    {
        if (_animator != null)
            _animator.Play(CloseAnimation);
        _isOpen = false;

        foreach (UnityEvent evento in onCloseEvents)
        {
            evento?.Invoke();
        }
    }

    public void ChangeLockText(string text)
    {
        LockedMessage = text;
    }
}