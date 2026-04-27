using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TriggerAction : MonoBehaviour
{
    [Header("Accion a ejecutar")]
    public List<UnityEvent> onPlayerEnter;

    public bool destroyAfterTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.IsGame())
        {
            foreach (var evt in onPlayerEnter)
            {
                evt.Invoke();
            }

            if (destroyAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
