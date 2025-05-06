using System.Collections;
using UnityEngine;
/// <summary>
/// Camera Controller
/// </summary>
public class CamController : MonoBehaviour
{
    [HideInInspector] public int worldSize;

    [Range(0f, 1f)]
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform playerTransform;

    private float orthoSize;
    [SerializeField] private float orthoSizeMultiplierLeft = 2.88f;
    [SerializeField] private float orthoSizeMultiplierRight = 2.98f;
    [SerializeField] private float orthoSizeMultiplierTop = 1f;
    [SerializeField] private float orthoSizeMultiplierBottom = 1f;
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

        pos.x = Mathf.Clamp(pos.x, 0 + (orthoSize * orthoSizeMultiplierLeft), worldSize - (orthoSize * orthoSizeMultiplierRight));
        pos.y = Mathf.Clamp(pos.y, 0 + (orthoSize * orthoSizeMultiplierBottom), worldSize - (orthoSize * orthoSizeMultiplierTop));
        GetComponent<Transform>().position = pos;
    }
}
