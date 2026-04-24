using UnityEngine;

public class JetpackController : MonoBehaviour
{
    [Header("Thrust")]
    public float MinThrust = 1f;
    public float MaxThrust = 20f;
    public float CurrentThrust = 5f;
    public float ThrustScrollStep = 1f;

    [Header("Movement")]
    public float MaxSpeed = 15f;
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

    private void OnEnable()
    {
        _velocity = Vector3.zero;
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

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        // Mantener derecha horizontal para que A/D no suba ni baje
        right.y = 0f;
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