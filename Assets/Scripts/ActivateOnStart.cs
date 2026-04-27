using UnityEngine;

public class ActivateOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpaceHelmetEffect>().EnableSpaceHelmetEffect();
    }
}
