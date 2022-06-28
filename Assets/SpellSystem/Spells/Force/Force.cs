using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : Spell
{
    [Header("Spell Parameter")]
    [SerializeField] private float forcePull = 800f;
    [SerializeField] private float upwardsForceScale = 0.3f;
    [SerializeField][Range(0f, 0.1f)] private float spreadAmount = 0.01f;
    [SerializeField] private LayerMask mask;

    public override void CastSpell()
    {
        RaycastHit hit;
        Vector3 pos = manager.AimDirection().position;
        Vector3[] directions = {
            new Vector3(0, 0, 1),
            new Vector3(spreadAmount, spreadAmount, 1),
            new Vector3(-spreadAmount, spreadAmount, 1),
            new Vector3(spreadAmount, -spreadAmount, 1),
            new Vector3(-spreadAmount, -spreadAmount, 1)
        };

        foreach (var direction in directions)
        {
            Debug.DrawLine(pos, pos + (manager.AimDirection().TransformDirection(direction) * 1000f), Color.green, 3f);
            if (Physics.Raycast(pos, manager.AimDirection().TransformDirection(direction), out hit, Mathf.Infinity, mask))
            {
                PullTowardsPlayer(hit.transform.gameObject);
                return;
            }
        }
    }

    private void PullTowardsPlayer(GameObject target)
    {
        var rigidbody = target.GetComponent<Rigidbody>();
        if (rigidbody == null) return;

        Vector3 forceDirection = Vector3.back + (upwardsForceScale * Vector3.up);
        rigidbody.AddForce(manager.AimDirection().TransformDirection(forceDirection) * forcePull);
    }

    void Draw(Vector3 pos, Vector3 direction)
    {
        Debug.DrawLine(pos, pos + (direction * 1000f), Color.green, 5f);
    }
}