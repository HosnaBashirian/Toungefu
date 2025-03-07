using UnityEngine;

public class PlayerTileMovements2 : MonoBehaviour
{
    // public GameObject player;
    
    public float moveSpeed = 5f;
    public Vector3 spawnPoint = new Vector3(0f, -2f, -40f);
    private Vector2Int lastInputDirection = Vector2Int.zero;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    private bool isMoving;
    
    void Start()
    {
        movePoint.parent = null;
        transform.position = spawnPoint;
        movePoint.position = spawnPoint;
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            isMoving = false;
            
            Vector2Int inputDirection = GetInputDirection();
            
            if (inputDirection != Vector2Int.zero && inputDirection != lastInputDirection)
            {
                Vector3 targetPosition = movePoint.position + new Vector3(inputDirection.x * 2f, 0f, inputDirection.y * 2f);
                
                if (Physics.OverlapBox(targetPosition, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, whatStopsMovement).Length == 0)
                {
                    movePoint.position = targetPosition;
                    isMoving = true;
                }
            }
            
            lastInputDirection = inputDirection;
        }
    }
    
    private Vector2Int GetInputDirection()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");
        
        if (horizontal != 0)
        {
            vertical = 0;
        }

        return new Vector2Int(horizontal, vertical);
    }
}
