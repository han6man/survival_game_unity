using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float smoothTime;
    [SerializeField] Transform playerTransform;

    private void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        GetComponent<Transform>().position = pos;
    }
}
