using UnityEngine;

public class PlayerTileMovements2 : MonoBehaviour
{
    // public GameObject player;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    public Vector3 spawnPoint = new Vector3(0f, -3f, -40f);
    public Vector3 respawnPoint = new Vector3(16f, -3f, 2f);

    private Vector3 targetDirection = -Vector3.right;
    private Quaternion targetRotation;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    private bool isMoving;

	public GameObject _gridManager;
	public Handler _handler;

	public Vector2Int coords;
	public Vector2Int gridFacing;

	private Vector2Int[] DIR_ENUM;
	public int facing_index = 0;

    void Start()
    {
        movePoint.parent = null;
        transform.position = spawnPoint;
        movePoint.position = spawnPoint;

        targetRotation = transform.rotation;

		_gridManager = GameObject.Find("GridManager");
		_handler = _gridManager.GetComponent<Handler>();

		DIR_ENUM = Handler.DIR_ENUM;
		facing_index = 3;
		gridFacing = DIR_ENUM[facing_index];
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

				facing_index--;
				if(facing_index < 0) facing_index = 3;
				gridFacing = DIR_ENUM[facing_index];
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                targetDirection = Quaternion.Euler(0, 90, 0) * targetDirection;
                UpdateTargetRotation();

				facing_index++;
				if(facing_index > 3) facing_index = 0;
				gridFacing = DIR_ENUM[facing_index];
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetDirection = Quaternion.Euler(0, 180, 0) * targetDirection;
                UpdateTargetRotation();

				gridFacing = _handler.CoordsScale(gridFacing, -1);
				for(short i = 0; i < 4; i++) if(_handler.CoordsCompare(DIR_ENUM[i], gridFacing)) facing_index = i;
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

					// End turn...
					_handler.ProcessTurn();
                }
            }
        }
    }

    private void UpdateTargetRotation()
    {
        targetRotation = Quaternion.LookRotation(targetDirection);
    }
}
