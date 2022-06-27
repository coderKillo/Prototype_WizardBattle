using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellInput : MonoBehaviour
{
    [SerializeField] private SpellManager manager;

    private void Start()
    {
        manager.ChangeSlot(0);
    }

    private void OnFirePrimary(InputValue value)
    {
        manager.FirePrimary();
    }

    private void OnFireSecondary(InputValue value)
    {
        manager.FireSecondary();
    }

    private void OnSlot0(InputValue value)
    {
        manager.ChangeSlot(0);
    }

    private void OnSlot1(InputValue value)
    {
        manager.ChangeSlot(1);
    }
}
