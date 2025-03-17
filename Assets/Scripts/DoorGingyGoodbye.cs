using UnityEngine;
using TMPro;

public class DoorGingyGoodbye : MonoBehaviour
{
    public GameObject gingy;
    public TextMeshProUGUI messageText;
    public AudioSource doorAudio;
    public AudioSource munchAudio;
    public float messageDuration = 2f;
    private bool messageShown = false;
    private bool doorOpened = false;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (gingy == null)
        {
            Debug.Log("Gingy is null triggering door open");
        }
        
        if (gingy== null && !messageShown && !doorOpened)
        {
            doorOpened = true;
            messageShown = true;
            ShowMessage("The door is now open.");
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
