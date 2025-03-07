using UnityEngine;

public class PlayerTileMovement : MonoBehaviour
{
    private CharacterController cc;
    
    public float tileSize = 2f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Vector3 targetPosition;
    private Vector3 lastDirection;

    private bool isGameStarted;
    private bool isMoving;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
        targetPosition = transform.position;
        lastDirection = transform.forward;
    }
  
    void Update()
    {
        HandleInput();
        MoveToTarget();
        RotatePlayer();
    }
    
    private void HandleInput()
    {
        if (isMoving) return;

        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputDirection = transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            // inputDirection = -transform.forward;
            lastDirection = -transform.forward;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // inputDirection = transform.right;
            lastDirection = transform.right;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // inputDirection = -transform.right;
            lastDirection = -transform.right;
            return;
        }
        
        if (inputDirection != Vector3.zero)
        {
            targetPosition = transform.position + inputDirection * tileSize;
            lastDirection = inputDirection;
        }

        if (IsPositionValid(targetPosition))
        {
            isMoving = true;
        }
        else
        {
            targetPosition = transform.position;
        }
        
        
    }

    

    private void MoveToTarget()
    {
        if (isMoving)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            
            cc.Move(movement);
    
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    private void RotatePlayer()
    {
        if (lastDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private bool IsPositionValid(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, position);
        Vector3 boxSize = new Vector3(tileSize * 0.9f, tileSize * 0.9f, tileSize * 0.9f);
        Vector3 boxCastOrigin = transform.position + Vector3.up * 0.1f;
        
        bool isBlocked = Physics.BoxCast(
            boxCastOrigin,
            boxSize / 2,
            direction,          
            Quaternion.identity,
            distance,           
            LayerMask.GetMask("Obstacles")
        );
        // if (isBlocked) Debug.Log("tile blocked");
        
        return !isBlocked;
    }
    
    void OnDrawGizmos() // for debugging ONLY
    {
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetPosition);
            Vector3 boxSize = new Vector3(tileSize * 0.9f, tileSize * 0.9f, tileSize * 0.9f);
            Vector3 boxCastOrigin = transform.position + Vector3.up * 0.1f;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(boxCastOrigin + direction * distance, boxSize);
        }
    }
}
