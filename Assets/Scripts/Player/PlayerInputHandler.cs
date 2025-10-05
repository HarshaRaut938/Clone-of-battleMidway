using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Events")]
    public System.Action<Vector2> OnMoveInput;
    
    private @PlayerInputActions inputActions;
    private Vector2 currentMoveInput;
    
    private void Awake()
    {
        inputActions = new @PlayerInputActions();
    }
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInputPerformed;
        inputActions.Player.Move.canceled += OnMoveInputCanceled;
    }
    
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMoveInputPerformed;
        inputActions.Player.Move.canceled -= OnMoveInputCanceled;
        inputActions.Player.Disable();
    }
    
    private void OnMoveInputPerformed(InputAction.CallbackContext context)
    {
        currentMoveInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(currentMoveInput);
    }
    
    private void OnMoveInputCanceled(InputAction.CallbackContext context)
    {
        currentMoveInput = Vector2.zero;
        OnMoveInput?.Invoke(currentMoveInput);
    }
    
    public Vector2 GetCurrentMoveInput()
    {
        return currentMoveInput;
    }
}
