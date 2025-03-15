using UnityEngine;
using TMPro;

public class DoorSecretLick : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    private bool messageShown = false;
    public int requiredLicks = 5;
    private int currentLicks = 0;

    private void Update()
    {
        if (currentLicks == 5 && !messageShown)
        {
            messageShown = true;
            ShowMessage("The door is now open. But there is nothing here.");
        }
        else messageShown = false;
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
        messageText.gameObject.SetActive(false);
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongue"))
        {
            currentLicks++;

            if (currentLicks >= requiredLicks)
            {
                Destroy(gameObject);
            }
        }
    }
}