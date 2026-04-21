using System.Collections;
using UnityEngine;

public class InheritParentTransform : MonoBehaviour
{
    private Vector3 _lastParentPosition;
    private Quaternion _lastParentRotation;
    private CharacterController _controller;
    private bool _started = false;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        _started = true;
        if (transform.parent.parent != null)
        {
            _lastParentPosition = transform.parent.parent.position;
            _lastParentRotation = transform.parent.parent.rotation;
        }
    }

    private void LateUpdate()
    {
        if (transform.parent.parent == null || !_started) return;

        Quaternion rotationDelta = transform.parent.parent.rotation * Quaternion.Inverse(_lastParentRotation);

        Vector3 offset = transform.position - transform.parent.parent.position;
        offset = rotationDelta * offset;

        Vector3 positionDelta = transform.parent.parent.position - _lastParentPosition;

        _controller.enabled = false;
        transform.position = transform.parent.parent.position + offset + positionDelta;
        transform.rotation = rotationDelta * transform.rotation;
        _controller.enabled = true;

        _lastParentPosition = transform.parent.parent.position;
        _lastParentRotation = transform.parent.parent.rotation;
    }
}