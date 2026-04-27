using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SCENE;

    [SerializeField]      
    private GameObject SCENETOUNLOAD;
    public void LoadScene()
    {
        SCENETOUNLOAD.SetActive(false);
        foreach (var a in SCENE)
        {
            a.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false; 
        GameManager.Instance.SetIsGame(true);
    }
}
