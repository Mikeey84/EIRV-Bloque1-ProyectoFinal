using UnityEngine;

public class MakeChild : MonoBehaviour
{
    void Start()
    {
        GameObject go = GameObject.Find("Blue Planet");
        transform.SetParent(go.transform);
    }
}
