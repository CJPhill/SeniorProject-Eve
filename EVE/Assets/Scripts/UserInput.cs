using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    public bool Interact { get; private set; }
    public bool MenuOpenClose { get; private set; }
    public bool InventoryOpenClose { get; private set; }


    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _menuOpenCloseAction;
    private InputAction _inventoryOpenCloseAction;

    private void Awake(){
        if(instance == null){ 
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void Update(){
        UpdateInput();
    }

    private void SetupInputActions(){ 
        _moveAction = _playerInput.actions["Move"];
        _interactAction = _playerInput.actions["Interact"];
        _menuOpenCloseAction = _playerInput.actions["MenuOpenClose"];
        _inventoryOpenCloseAction = _playerInput.actions["InventoryOpenClose"];
    }

    private void UpdateInput(){
        MoveInput = _moveAction.ReadValue<Vector2>();
        Interact = _interactAction.WasPressedThisFrame();
        MenuOpenClose = _menuOpenCloseAction.WasPressedThisFrame();
        InventoryOpenClose = _inventoryOpenCloseAction.WasPressedThisFrame();
    }
}
