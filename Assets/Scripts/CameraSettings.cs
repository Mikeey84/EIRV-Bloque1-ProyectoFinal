using UnityEngine;
using Unity.Cinemachine;

public class CameraSettings : MonoBehaviour
{
    public CinemachineCamera VirtualCamera;
    public float FarClipPlane = 5000f;
    public float FieldOfView = 60f;

    private void OnEnable()
    {
        LensSettings lens = VirtualCamera.Lens;
        lens.FarClipPlane = FarClipPlane;
        lens.FieldOfView = FieldOfView;
        VirtualCamera.Lens = lens;
    }
}