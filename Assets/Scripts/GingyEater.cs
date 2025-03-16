using UnityEngine;
using TMPro;

public class GingyEater : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongue"))
        {
            ShowMessage("Nooooooooooooo!");
            
            Destroy(gameObject);
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