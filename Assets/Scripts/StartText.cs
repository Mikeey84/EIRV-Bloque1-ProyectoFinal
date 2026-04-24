using UnityEngine;

public class StartText : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private float displayMessage;
    bool active = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!active)
        {
            InteractMessageScript.Instance.ShowMessage(message, displayMessage);
            active = true;
        }
    }
}
