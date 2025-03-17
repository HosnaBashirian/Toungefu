using UnityEngine;
using TMPro;

public class DoorEatBonbon : MonoBehaviour
{
    public GameObject bonBon;
    public TextMeshProUGUI messageText;
    public AudioSource doorAudio;
    public AudioSource munchAudio;
    public float messageDuration = 2f;
    private bool messageShown = false;
    private bool doorOpened = false;

    private void Update()
    {
        if (bonBon == null && !messageShown && !doorOpened)
        {
            doorOpened = true;
            messageShown = true;
            ShowMessage("The door is now open. You may proceed to the next puzzle.");
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

    void PlayMunchSound()
    {
        if (munchAudio)
        {
            munchAudio.Play();
        }
    }
}