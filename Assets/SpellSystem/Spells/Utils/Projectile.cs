using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    public float Speed { set { speed = value; } }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
