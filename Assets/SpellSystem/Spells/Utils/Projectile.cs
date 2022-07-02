using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private float speed;
    public float Speed { set { speed = value; } }

    private LayerMask hitMask;
    public LayerMask HitMask { set { hitMask = value; } }

    private int bounceCount = 0;
    public int BounceCount { get { return bounceCount; } }

    private UnityEvent onDestroyMissile = new UnityEvent();
    public UnityEvent OnDestroyMissile { get { return onDestroyMissile; } }

    private UnityEvent<RaycastHit> onHit = new UnityEvent<RaycastHit>();
    public UnityEvent<RaycastHit> OnHit { get { return onHit; } }

    private void Update()
    {
        var distance = speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, hitMask))
        {
            onHit.Invoke(hit);
        }
        else
        {
            transform.position += transform.forward * distance;
        }
    }

    public void DestroyMissile()
    {
        Destroy(gameObject);
    }

    public void Bounce(Vector3 collisionNormal)
    {
        var direction = Vector3.Reflect(transform.forward.normalized, collisionNormal);
        transform.forward = direction;
        bounceCount++;
    }

    private void OnDisable()
    {
        onDestroyMissile.Invoke();
    }
}
