using UnityEngine;

public class CameraFollowHead : MonoBehaviour
{
    public Transform head;

    void LateUpdate()
    {
        transform.position = head.position;
        transform.rotation = head.rotation;
    }
}