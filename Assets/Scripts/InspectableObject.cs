using StarterAssets;
using UnityEngine;

public class InspectableObject : Interactable
{
    [Header("Configuración de Inspección")]
    public float distanciaDeLaCamara = 1.5f;
    public float sensibilidadRotacion = 500f;
    public string mensajeAlMirar = "Presiona E para inspeccionar";

    [Header("Referencias (Opcional)")]
    // Si dejas estos vacíos, el script intentará buscarlos automáticamente
    public CameraController cameraController;
    public MonoBehaviour firstPersonController;

    private bool isInspecting = false;
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;
    private Transform camaraTransform;
    private Transform transformOriginalParent;

    private void Start()
    {
        camaraTransform = Camera.main.transform;
    }

    public override void ShowMess()
    {
        if (!isInspecting)
        {
            // Aquí podrías conectar con tu sistema de UI de texto
            Debug.Log(mensajeAlMirar);
        }
    }

    public override void Interact()
    {
        if (!isInspecting)
            StartInspecting();
        else
            StopInspecting();
    }

    void StartInspecting()
    {
        isInspecting = true;
        PlayerInteraction.IsInspecting = true;
        // Guardar estado original
        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
        transformOriginalParent = transform.parent;

        // Desactivar controles
        SetPlayerControls(false);

        // Mover el objeto frente a la cámara
        transform.SetParent(camaraTransform);
        transform.localPosition = new Vector3(0, 0, distanciaDeLaCamara);
    }

    void StopInspecting()
    {
        isInspecting = false;
        PlayerInteraction.IsInspecting = false;
        // Devolver a su lugar (o dejarlo donde estaba si es un objeto físico)
        transform.SetParent(transformOriginalParent);
        transform.position = posicionOriginal;
        transform.rotation = rotacionOriginal;

        // Reactivar controles
        SetPlayerControls(true);
    }

    private void Update()
    {
        if (isInspecting)
        {
            // Rotar el objeto con el ratón
            float rotX = Input.GetAxis("Mouse X") * sensibilidadRotacion * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * sensibilidadRotacion * Time.deltaTime;

            transform.Rotate(camaraTransform.up, -rotX, Space.World);
            transform.Rotate(camaraTransform.right, rotY, Space.World);

            // Permitir soltar con E o Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopInspecting();
            }
        }
    }

    private void SetPlayerControls(bool state)
    {
        if (cameraController != null) cameraController.enabled = state;
        if (firstPersonController != null) firstPersonController.enabled = state;

        // Bloquear/Desbloquear cursor
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }
}