using UnityEngine;

public class PlayerTileMovements2 : MonoBehaviour
{
    // public GameObject player;
    
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    
    public Vector3 spawnPoint = new Vector3(0f, -2f, -40f);
    private Vector3 targetDirection = Vector3.forward;
    private Quaternion targetRotation;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    private bool isMoving;
    
    void Start()
    {
        movePoint.parent = null;
        transform.position = spawnPoint;
        movePoint.position = spawnPoint;

        targetRotation = transform.rotation;
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            isMoving = false;
            
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                targetDirection = Quaternion.Euler(0, -90, 0) * targetDirection;
                UpdateTargetRotation();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                targetDirection = Quaternion.Euler(0, 90, 0) * targetDirection;
                UpdateTargetRotation();
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetDirection = Quaternion.Euler(0, 180, 0) * targetDirection;
                UpdateTargetRotation();
            }

            // handle forward movement 
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isMoving)
            {
                Vector3 targetPosition = movePoint.position + targetDirection * 2f; // move forward in the facing direction

                // check if the target position has no obstacles
                if (Physics.OverlapBox(targetPosition, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, whatStopsMovement).Length == 0)
                {
                    movePoint.position = targetPosition;
                    isMoving = true;
                }
            }
        }
    }

    
    // private Vector2Int GetInputDirection()
    // {
    //     int horizontal = (int)Input.GetAxisRaw("Horizontal");
    //     int vertical = (int)Input.GetAxisRaw("Vertical");
    //     
    //     if (horizontal != 0)
    //     {
    //         vertical = 0;
    //     }
    //
    //     return new Vector2Int(horizontal, vertical);
    // }

    private void UpdateTargetRotation()
    {
        targetRotation = Quaternion.LookRotation(targetDirection);
    }
}
