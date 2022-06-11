using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothTargetTime;

    private Vector3 offset;
    private Vector3 smoothDampVelocity;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void Update()
    {
        Vector3 targetCamera = player.position + offset;
        transform.position =
            Vector3.SmoothDamp(transform.position, targetCamera, ref smoothDampVelocity, smoothTargetTime);
    }
}
