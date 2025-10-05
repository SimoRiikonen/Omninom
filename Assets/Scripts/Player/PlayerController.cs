using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    InputAction moveAction;
    InputAction backAction;

    private void OnValidate()
    {
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement-component not found.");
        }
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        backAction = InputSystem.actions.FindAction("Back");
    }

    private void Update()
    {
        var moveValue = moveAction.ReadValue<Vector2>();

        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        playerMovement.MovePlayer(movement);

        var backValue = backAction.ReadValue<float>();

        if (backValue > 0.5f)
        {
            EventManager.TriggerEvent(Events.GamePaused, null);
        }
    }
}