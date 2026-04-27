using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    private int rotationCount = 0;
    public static GameManager Instance { get; private set; }
    public List<UnityEvent> onRotationLimitReached;

    [Header("Camera")]
    public Camera mainCamera;
    public CinemachineBrain brain;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Look At")]
    public Transform lookAtTarget;

    [Header("Camera Shake")]
    public float shakeMagnitude = 0.1f;
    public UIAndParentController UIAndParentController;

    public GameObject playerTrigger;
    public JetpackController controller;
    public StarterAssetsInputs starterAssetsInputs;
    public GameObject playerplayer;
    public Transform spawnT;

    private Coroutine shakeCoroutine;
    private Vector3 originalCameraLocalPos;

    private bool isGame = false;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
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

    public void AddForce()
    {
        controller.AddForceY();
    }

    public void EndGame()
    {
        playerTrigger.SetActive(true);
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
        if (rotationCount == 2)
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
        if (brain != null)
            brain.enabled = false;
        shakeCoroutine = StartCoroutine(ShakeCameraInfinite());
    }

    public void StopCameraShake()
    {
        if (shakeCoroutine == null) return;
        StopCoroutine(shakeCoroutine);
        shakeCoroutine = null;
        mainCamera.transform.localPosition = originalCameraLocalPos;
        if (brain != null)
            brain.enabled = true;
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
        SetLookAtTargetWithHardLookAt(lookAtTarget);
    }

    public void SetLookAtTarget(Transform target)
    {
        if (virtualCamera == null) return;
        SetLookAtTargetWithHardLookAt(target);
    }

    private void SetLookAtTargetWithHardLookAt(Transform target)
    {
        virtualCamera.LookAt = target;

        // Obtener o crear el componente CinemachineHardLookAt
        var hardLookAt = virtualCamera.GetCinemachineComponent<CinemachineHardLookAt>();

        if (hardLookAt == null)
        {
            // Si no existe, añadirlo (esto reemplaza el componente Aim actual)
            hardLookAt = virtualCamera.AddCinemachineComponent<CinemachineHardLookAt>();
        }

        // El HardLookAt ya está configurado, usa automáticamente el LookAt target
        Debug.Log("HardLookAt configurado en la cámara virtual");
    }

    public void ClearLookAt()
    {
        if (virtualCamera == null) return;
        virtualCamera.LookAt = null;
    }

    public bool IsGame()
    {
        return isGame;
    }   
    public void SetIsGame(bool value)
    {
        isGame = value;
        starterAssetsInputs.CursorLock();
        playerplayer.transform.position = spawnT.position;
    }
}