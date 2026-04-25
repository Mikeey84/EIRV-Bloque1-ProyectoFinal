using StarterAssets;
using System.Collections;
using UnityEngine;

public class InspectableObject : Interactable
{
    [Header("Configuración de Inspección")]
    public float distanciaDeLaCamara = 1.5f;
    public float sensibilidadRotacion = 500f;
    public string mensajeAlMirar = "Presiona E para coger";

    [Header("Referencias (Opcional)")]
    // Si dejas estos vacíos, el script intentará buscarlos automáticamente
    public CameraController cameraController;
    public FirstPersonController firstPersonController;

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
            InteractMessageScript.Instance.ShowShortMessage(mensajeAlMirar);
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

        InteractMessageScript.Instance.Hide();

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
        InteractMessageScript.Instance.Hide();
        isInspecting = false;
        PlayerInteraction.IsInspecting = false;
        SetPlayerControls(true);

        DestroyGameObject();
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                InteractMessageScript.Instance.Hide();
                gameObject.layer = 0;
                StopInspecting();
            }
        }
    }

    private void SetPlayerControls(bool state)
    {
        cameraController.enabled = state;
        firstPersonController.canMove = state;
    }
}