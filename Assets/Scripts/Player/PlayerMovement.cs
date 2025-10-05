using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float padding = 0.5f;
    
    [Header("References")]
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    
    private Camera mainCamera;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private bool boundsCalculated = false;
    
    private void Awake()
    {
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }
    
    private void Start()
    {
        CalculateScreenBounds();
    }
    
    public void Move(Vector2 inputDirection)
    {
        if (inputDirection == Vector2.zero) return;
        Vector2 deltaMovement = inputDirection * moveSpeed * Time.deltaTime;
        Vector2 newPosition = new Vector2(
            transform.position.x + deltaMovement.x,
            transform.position.y + deltaMovement.y
        );
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        
        transform.position = newPosition;
    }
    
    private void CalculateScreenBounds()
    {
        if (mainCamera == null || boundsCalculated) return;
        
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        
        Vector2 spriteSize = playerSpriteRenderer.bounds.size;
        
        minBounds = new Vector2(
            bottomLeft.x + spriteSize.x / 2 + padding,
            bottomLeft.y + spriteSize.y / 2 + padding
        );
        
        maxBounds = new Vector2(
            topRight.x - spriteSize.x / 2 - padding,
            topRight.y - spriteSize.y / 2 - padding
        );
        
        boundsCalculated = true;
    }
    
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
    public void SetPadding(float newPadding)
    {
        padding = newPadding;
        boundsCalculated = false; 
        CalculateScreenBounds();
    }
}
