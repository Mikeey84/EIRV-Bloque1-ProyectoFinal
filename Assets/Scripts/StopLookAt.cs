using System.Collections.Generic;
using UnityEngine;

public class StopLookAt : MonoBehaviour
{
    [SerializeField] private List<GameObject> sceneObjects;
    [SerializeField] private GameObject THEFATHER;
    public void StopLookAtPLS()
    {
        GameManager.Instance.ClearLookAt();
    }

    public void CanvasActivation()
    {
        GameManager.Instance.UIAndParentController.ActivateCanvasAndReparent();
    }

    public void CanvasDeActivation()
    {
        GameManager.Instance.UIAndParentController.DeactivateCanvas();
    }

    public void ChangeParent()
    {
        foreach(GameObject go in sceneObjects)
        {
            go.transform.SetParent(THEFATHER.transform);
        }
    }

    public void EndScene()
    {
        GameManager.Instance.EndGame();
    }

    public void AddForce()
    {
        GameManager.Instance.AddForce();
    }
}
