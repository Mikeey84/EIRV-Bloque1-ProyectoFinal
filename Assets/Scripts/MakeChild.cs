using UnityEngine;

public class MakeChild : MonoBehaviour
{
    void Start()
    {
        Camera.main.transform.SetParent(transform);
    }
}
