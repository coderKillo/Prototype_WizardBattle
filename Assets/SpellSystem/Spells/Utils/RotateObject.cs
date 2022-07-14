using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis;
    [SerializeField] private float rotationPerSeconds;

    void Update()
    {
        transform.Rotate(rotationAxis.normalized, rotationPerSeconds * 360f * Time.deltaTime);
    }
}
