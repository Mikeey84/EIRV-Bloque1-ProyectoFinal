using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float InteractDistance = 3f;
    public LayerMask InteractableLayers;
    public Camera _camera;

    public static bool IsInspecting = false;

    private void Update()
    {
        if (IsInspecting) return;

        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractDistance, InteractableLayers))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                interactable.ShowMess();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
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