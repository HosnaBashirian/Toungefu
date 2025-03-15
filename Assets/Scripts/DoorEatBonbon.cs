using UnityEngine;
using TMPro;

public class DoorEatBonbon : MonoBehaviour
{
    public GameObject bonBon;
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    private bool messageShown = false;

    private void Update()
    {
        if (bonBon == null && !messageShown) 
        {
            messageShown = true;
            ShowMessage("The door is now open. You may proceed to the next puzzle.");
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
            Destroy(gameObject);
        }
    }
}