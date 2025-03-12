using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [HideInInspector] public int worldSize;

    [Range(0f, 1f)]
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform playerTransform;

    private float orthoSize;

    public void Spawn(Vector3 pos)
    {
        GetComponent<Transform>().position = pos;
        orthoSize = GetComponent<Camera>().orthographicSize;
    }

    private void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        pos.x = Mathf.Clamp(pos.x, 0 + (orthoSize * 2.365f), worldSize - (orthoSize * 2.365f));

        GetComponent<Transform>().position = pos;
    }
}
