using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float InteractDistance = 3f;
    public LayerMask InteractableLayers;

    public Camera _camera;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, InteractDistance, InteractableLayers))
            {
                Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
                if (interactable != null)
                    interactable.Interact();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_camera == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * InteractDistance);
    }
}