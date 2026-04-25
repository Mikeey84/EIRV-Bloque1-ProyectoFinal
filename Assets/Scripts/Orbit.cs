using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float radius = 5f;
    public float speed = 250f;

    public float startOffset;

    private float angle = 0f;

    void Start()
    {
        angle = startOffset;
    }

    void Update()
    {
        if (target == null) return;

        angle += speed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;

        transform.position = target.position + new Vector3(x, 0, z);
        transform.rotation = Quaternion.Euler(0f, -angle + 90f, 0f);
    }
}