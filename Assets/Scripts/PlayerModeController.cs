using UnityEngine;

public class PlayerModeController : MonoBehaviour
{
    public static PlayerModeController Instance { get; private set; }

    [Tooltip("True = modo a pie, False = modo jetpack")]
    public bool Walk = true;

    [SerializeField] private StarterAssets.FirstPersonController walkController;
    [SerializeField] private JetpackController jetpackController;

    private bool _lastWalk;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _lastWalk = !Walk;
        UpdateMode();
    }

    private void Update()
    {
        if (Walk != _lastWalk)
        {
            UpdateMode();
            _lastWalk = Walk;
        }
    }

    private void UpdateMode()
    {
        walkController.enabled = Walk;
        jetpackController.enabled = !Walk;
    }

    public void SetWalkMode() => Walk = true;
    public void SetJetpackMode() => Walk = false;
}