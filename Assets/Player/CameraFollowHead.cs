using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowHead : MonoBehaviour
{
    public Transform head;

    void LateUpdate()
    {
        transform.position = head.position;
    }
}