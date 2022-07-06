using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class SpellInput : MonoBehaviour
{
    [SerializeField] private SpellManager manager;

    private PlayerInput input;

    public InputAction FirePrimary { get { return firePrimaryAction; } }
    private InputAction firePrimaryAction;
    public InputAction FireSecondary { get { return fireSecondaryAction; } }
    private InputAction fireSecondaryAction;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        firePrimaryAction = input.actions["FirePrimary"];
        fireSecondaryAction = input.actions["FireSecondary"];

        firePrimaryAction.canceled += (InputAction.CallbackContext context) => { manager.FirePrimaryCanceled(); };
        fireSecondaryAction.canceled += (InputAction.CallbackContext context) => { manager.FireSecondaryCanceled(); };

        manager.ChangeSlot(0);
    }

    private void OnFirePrimary(InputValue value)
    {
        manager.FirePrimary();
    }

    private void OnFireSecondary(InputValue value)
    {
        // TODO: find solution to pass input value isPressed, to SpellManager
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
