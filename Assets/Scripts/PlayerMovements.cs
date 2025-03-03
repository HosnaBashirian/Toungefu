using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    public Camera playerCamera;
    CharacterController cc;
    
    public float moveSpeed = 7.0f;
    public float jumpPower = 10.0f;
    public float gravity = 10.0f;
    
    public float lookSensitivity = 2.0f;
    public float lookXLimit = 45.0f;
    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    
    Vector3 moveDirection = Vector3.zero;
    
    public bool canMove = true;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canMove)
        {
            #region movement
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            
            float currentSpeedX = Input.GetAxis("Vertical") * moveSpeed;
            float currentSpeedY = Input.GetAxis("Horizontal") * moveSpeed;
            float moveDirectionY = moveDirection.y;
            
            moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);
            
            #endregion
            
            #region jump
            if (Input.GetButton("Jump") && cc.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = moveDirectionY;
            }

            if (!cc.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            #endregion
            
            cc.Move(moveDirection * Time.deltaTime);
            
            #region rotation
            rotationX += -Input.GetAxis("Mouse Y") * lookSensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            rotationY = Input.GetAxis("Mouse X") * lookSensitivity;

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, rotationY, 0);

            #endregion

        }
    }
}
