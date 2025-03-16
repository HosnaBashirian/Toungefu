// using UnityEngine;
// using TMPro;
//
// public class DoorLickPuzzle : MonoBehaviour
// {
//     public TextMeshProUGUI messageText;
//     public float messageDuration = 2f;
//     private bool messageShown = false;
//
//     public string[] correctOrder = { "Red", "Yellow", "Green", "Blue" };
//     private int currentOrderIndex = 0;
//
//     private void Update()
//     {
//         // Check if the puzzle is solved and display a message
//         if (currentOrderIndex == correctOrder.Length && !messageShown)
//         {
//             messageShown = true;
//             ShowMessage("The door is now open. You may proceed to the next puzzle.");
//         }
//     }
//
//     void ShowMessage(string message)
//     {
//         if (messageText != null)
//         {
//             messageText.text = message;
//             messageText.gameObject.SetActive(true);
//
//             Invoke("HideMessage", messageDuration);
//         }
//     }
//
//     void HideMessage()
//     {
//         if (messageText != null)
//         {
//             messageText.gameObject.SetActive(false);
//         }
//         
//         Destroy(gameObject);
//     }
//
//     public void LickLollipop(string lollipopColor)
//     {
//         // Check if the licked lollipop is the next in the correct order
//         if (lollipopColor == correctOrder[currentOrderIndex])
//         {
//             currentOrderIndex++;
//             // Debug.Log("Correct! Licked " + lollipopColor + ". Progress: " + currentOrderIndex + "/" + correctOrder.Length);
//             
//             if (currentOrderIndex == correctOrder.Length)
//             {
//                 // Debug.Log("All lollipops licked in the correct order! Door unlocked!");
//             }
//         }
//         else
//         {
//             currentOrderIndex = 0;
//             ShowMessage("Wrong order! Try again");
//         }
//     }
// }

using UnityEngine;
using TMPro;

public class DoorLickPuzzle : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    private bool messageShown = false;

    public string[] correctOrder = { "Red", "Yellow", "Green", "Blue" };
    private int currentOrderIndex = 0;

    public float raycastDistance = 10f;
    public LayerMask lollipopLayer;

    private void Update()
    {
        if (currentOrderIndex == correctOrder.Length && !messageShown)
        {
            messageShown = true;
            ShowMessage("The door is now open. You may proceed to the next puzzle.");
            OpenDoor();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            PerformRaycast();
        }
    }

    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);

            Invoke("HideMessage", messageDuration);
        }
    }

    void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    void OpenDoor()
    {
        HideMessage();
        Destroy(gameObject);
        Debug.Log("Door has been destroyed!");
    }

    void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 
                raycastDistance, lollipopLayer))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            
            Lollipop lollipop = hit.collider.GetComponent<Lollipop>();
            if (lollipop != null)
            {
                Debug.Log("Licked lollipop: " + lollipop.color);
                LickLollipop(lollipop.color);
            }
            else
            {
                Debug.LogWarning("Hit object does not have a Lollipop component!");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    public void LickLollipop(string lollipopColor)
    {
        if (lollipopColor == correctOrder[currentOrderIndex])
        {
            currentOrderIndex++;
            Debug.Log("Correct! Licked " + lollipopColor + ". Progress: " + currentOrderIndex + "/" + correctOrder.Length);
            
            if (currentOrderIndex == correctOrder.Length)
            {
                Debug.Log("All lollipops licked in the correct order! Door unlocked!");
            }
        }
        else
        {
            // Reset the sequence if the wrong lollipop is licked
            currentOrderIndex = 0;
            Debug.Log("Wrong order! Sequence reset.");
            ShowMessage("Wrong order! Try again.");
        }
    }
    
    // private void OnDrawGizmos()
    // {
    //     if (Camera.main != null)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * raycastDistance);
    //     }
    }