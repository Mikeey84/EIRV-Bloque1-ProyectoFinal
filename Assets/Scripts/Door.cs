using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Door : Interactable
{
    [Header("Door")]
    public string OpenAnimation;
    public string CloseAnimation;
    public string OpenMessage;
    public string CloseMessage;
    public string LockedMessage;
    public bool locked;

    [Header("Audio")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;

    [Header("Events")]
    public List<UnityEvent> onOpenEvents = new List<UnityEvent>();
    public List<UnityEvent> onCloseEvents = new List<UnityEvent>();

    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        if (locked)
        {
            InteractMessageScript.Instance.ShowShortMessage(LockedMessage, 1.5f);
            PlaySound(lockedSound);
            return;
        }

        Debug.Log(_isOpen ? "Closing door" : "Opening door");

        _isOpen = !_isOpen;
        _animator.Play(_isOpen ? OpenAnimation : CloseAnimation);

        if (_isOpen)
        {
            PlaySound(openSound);

            foreach (UnityEvent evento in onOpenEvents)
            {
                evento?.Invoke();
            }
        }
        else
        {
            PlaySound(closeSound);

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
        PlaySound(closeSound);

        foreach (UnityEvent evento in onCloseEvents)
        {
            evento?.Invoke();
        }
    }

    public void ChangeLockText(string text)
    {
        LockedMessage = text;
    }

    private void PlaySound(AudioClip clip)
    {
        if (_audioSource == null || clip == null) return;

        _audioSource.PlayOneShot(clip);
    }
}