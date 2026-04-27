using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    private int rotationCount = 0;

    public static GameManager Instance { get; private set; }

    public List<UnityEvent> onRotationLimitReached;

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Look At")]
    public Transform lookAtTarget;

    [Header("Camera Shake")]
    public float shakeMagnitude = 0.1f;

    public UIAndParentController UIAndParentController;

    private Coroutine shakeCoroutine;
    private Vector3 originalCameraLocalPos;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddRotation(GameObject parent)
    {
        if (rotationCount < 2)
        {
            rotationCount++;
        }
        if(rotationCount == 2)
        {
            UIAndParentController.newParent = parent.transform;

            foreach (var unityEvent in onRotationLimitReached)
            {
                unityEvent?.Invoke();
            }
        }
    }

    // ================= CAMERA SHAKE =================

    public void CameraShake()
    {
        if (shakeCoroutine != null) return;

        originalCameraLocalPos = mainCamera.transform.localPosition;
        shakeCoroutine = StartCoroutine(ShakeCameraInfinite());
    }

    public void StopCameraShake()
    {
        if (shakeCoroutine == null) return;

        StopCoroutine(shakeCoroutine);
        shakeCoroutine = null;

        mainCamera.transform.localPosition = originalCameraLocalPos;
    }

    private IEnumerator ShakeCameraInfinite()
    {
        while (true)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalCameraLocalPos + new Vector3(x, y, 0f);

            yield return null;
        }
    }

    // ================= CINEMACHINE LOOK AT =================

    public void SetLookAtTarget()
    {
        if (virtualCamera == null || lookAtTarget == null) return;

        virtualCamera.LookAt = lookAtTarget;
    }

    public void SetLookAtTarget(Transform target)
    {
        if (virtualCamera == null) return;

        virtualCamera.LookAt = target;
    }

    public void ClearLookAt()
    {
        if (virtualCamera == null) return;

        virtualCamera.LookAt = null;
    }
}