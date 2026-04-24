using UnityEngine;

public class JetpackController : MonoBehaviour
{
    [Header("Thrust")]
    [Tooltip("Fuerza mínima del propulsor")]
    public float MinThrust = 1f;
    [Tooltip("Fuerza máxima del propulsor")]
    public float MaxThrust = 20f;
    [Tooltip("Fuerza actual del propulsor")]
    public float CurrentThrust = 5f;
    [Tooltip("Cuánto cambia la fuerza por tick de rueda")]
    public float ThrustScrollStep = 1f;

    [Header("Movement")]
    [Tooltip("Velocidad máxima alcanzable")]
    public float MaxSpeed = 15f;
    [Tooltip("Cuánto frena al soltar teclas (0 = no frena, 1 = para instantáneo)")]
    [Range(0f, 1f)]
    public float Damping = 0.95f;

    private CharacterController _controller;
    private Vector3 _velocity = Vector3.zero;
    private Camera _camera;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleScrollWheel();
        HandleMovement();
    }

    private void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) CurrentThrust += ThrustScrollStep;
        else if (scroll < 0f) CurrentThrust -= ThrustScrollStep;
        CurrentThrust = Mathf.Clamp(CurrentThrust, MinThrust, MaxThrust);
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Ejes de movimiento relativos a donde mira la cámara
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        // Sin componente vertical para que WASD no suba/baje por mirar arriba/abajo
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 inputDirection = forward * v + right * h;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            _velocity += inputDirection.normalized * CurrentThrust * Time.deltaTime;
            _velocity = Vector3.ClampMagnitude(_velocity, MaxSpeed);
        }
        else
        {
            _velocity *= Mathf.Pow(1f - Damping, Time.deltaTime * 60f);
        }

        _controller.Move(_velocity * Time.deltaTime);
    }
}