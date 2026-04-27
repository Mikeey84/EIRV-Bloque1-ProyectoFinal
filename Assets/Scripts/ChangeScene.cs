using System.Collections;
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
        foreach (var a in SCENE)
        {
            a.SetActive(true);
        }

        StartCoroutine(Deactivate());
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false; 
    }
    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.1f);
        foreach(var a in SCENETOUNLOAD.GetComponentsInChildren<Transform>(true))
        {
            a.gameObject.SetActive(false);
        }
        GameManager.Instance.SetIsGame(true);
    }
}
