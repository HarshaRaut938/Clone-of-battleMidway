using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerShooting shooting;
    
    private void Awake()
    {
        if (inputHandler == null)
            inputHandler = GetComponent<PlayerInputHandler>();
        if (movement == null)
            movement = GetComponent<PlayerMovement>();
        if (shooting == null)
            shooting = GetComponent<PlayerShooting>();
    }
    
    private void Update()
    {
        if (inputHandler != null && movement != null)
        {
            Vector2 moveInput = inputHandler.GetCurrentMoveInput();
            movement.Move(moveInput);
        }
    }
}
