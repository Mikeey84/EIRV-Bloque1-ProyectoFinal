using UnityEngine;

public class Door : Interactable
{
    [Header("Door")]
    public string OpenAnimation;
    public string CloseAnimation;

    private Animator _animator;
    private bool _isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        Debug.Log(_isOpen ? "Closing door" : "Opening door");
        _isOpen = !_isOpen;
        _animator.Play(_isOpen ? OpenAnimation : CloseAnimation);
    }
    public void SetAnimation(string Animation)
    {
        if (_animator != null)
            _animator.Play(Animation);
    }
}