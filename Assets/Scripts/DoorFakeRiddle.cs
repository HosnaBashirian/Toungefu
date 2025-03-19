using UnityEngine;
using TMPro;

public class LickLockController : MonoBehaviour
{
    public GameObject lockObject;
    public GameObject doorObject;
    public TextMeshProUGUI messageText;
    public AudioSource doorAudio;
    public AudioSource munchAudio;
    public float messageDuration = 2f;

    private bool isLockDestroyed = false;
    private bool doorOpened = false;

    private void Update()
    {

        if (lockObject == null && !isLockDestroyed && !doorOpened)
        {
            doorOpened = true;
            isLockDestroyed = true;
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
      messageText.gameObject.SetActive(false);

      PlayMunchSound();
      Destroy(gameObject);
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
