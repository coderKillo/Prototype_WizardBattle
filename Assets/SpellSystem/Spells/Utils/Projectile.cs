using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private float speed;
    public float Speed { set { speed = value; } }

    private LayerMask bounceMask;
    public LayerMask BounceMask { set { bounceMask = value; } }

    private int bounceCount = 0;
    public int BounceCount { set { bounceCount = value; } }

    private UnityEvent onDestroyMissile = new UnityEvent();
    public UnityEvent OnDestroyMissile { get { return onDestroyMissile; } }

    private UnityEvent<RaycastHit> onHit = new UnityEvent<RaycastHit>();
    public UnityEvent<RaycastHit> OnHit { get { return onHit; } }

    private void Update()
    {
        var distance = speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, bounceMask))
        {
            transform.position = hit.point;
            Bounce(hit.normal);

            onHit.Invoke(hit);

            if (bounceCount <= 0)
            {
                DestroyMissile();
            }
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

    private void OnDestroy()
    {
        onDestroyMissile.Invoke();
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var direction = Vector3.Reflect(transform.forward.normalized, collisionNormal);
        transform.forward = direction;
        bounceCount--;
    }
}
