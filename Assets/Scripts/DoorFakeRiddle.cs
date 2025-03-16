using UnityEngine;
using TMPro;

public class LickLockController : MonoBehaviour
{
    public GameObject lockObject;
    public GameObject doorObject;
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;

    private bool isLockDestroyed = false;

    private void Update()
    {
        if (lockObject == null && !isLockDestroyed)
        {
            isLockDestroyed = true;
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongue"))
        {
            if (lockObject != null)
            {
                Destroy(lockObject);
                Debug.Log("Lock has been destroyed!");
            }
        }
    }

    void OpenDoor()
    {
        if (messageText != null)
        {
            ShowMessage("The door is now open!");
        }
        
        if (doorObject != null)
        {
            Destroy(doorObject);
            Debug.Log("Door has been opened!");
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
}