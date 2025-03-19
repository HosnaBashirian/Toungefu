using UnityEngine;
using TMPro;

public class DoorJawBreaker : MonoBehaviour
{
    public GameObject jawBreaker;
    public TextMeshProUGUI messageText;
    public AudioSource doorAudio;
    public float messageDuration = 2f;
    private bool messageShown = false;
    private bool doorOpened = false;

    private void Update()
    {
        if (jawBreaker == null && !messageShown && !doorOpened)
        {
            doorOpened = true;
            messageShown = true;
            ShowMessage("JAWBREAKER VANQUISHED");
            PlayDoorSound();
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

    void PlayDoorSound()
    {
        if (doorAudio)
        {
            doorAudio.Play();
        }
    }
}
