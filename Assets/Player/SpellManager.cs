using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    [SerializeField] private Transform rayCastRef;
    [SerializeField] private LayerMask mask;

    [Header("Spell 1")]
    [SerializeField] private float forceSpell1 = 2000f;
    [SerializeField] private float upwardsForceScale = 1f;

    private void OnFire(InputValue value)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayCastRef.position, rayCastRef.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        {
            Vector3 forceDirection = Vector3.back + (upwardsForceScale * Vector3.up);
            hit.rigidbody.AddForce(rayCastRef.TransformDirection(forceDirection) * forceSpell1);
        }
    }
}
