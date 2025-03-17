using System;
using UnityEngine;
using TMPro;

public class GingyEater : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    public AudioSource munchSound;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tongue"))
        {
            ShowMessage("Nooooooooooooo!");

            PlayMunchSound();
            DestroyAllChildren();
            Destroy(gameObject, messageDuration + 0.1f);
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
        else
        {
            Debug.LogError("messageText is not assigned!");
        }
    }

    void HideMessage()
    {
        if (messageText != null)
        {
            Debug.Log("hiding message");
            messageText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("messageText is null in HideMessage!"); // Debug log
        }
    }
    
    void DestroyAllChildren()
    {
        // GameObject parentEatable = transform.parent.gameObject;
        //
        // if (parentEatable != null)
        // {
        //     Destroy(parentEatable);
        // }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        } 
    }
    
    void PlayMunchSound()
    {
        if (munchSound)
        {
            munchSound.Play();
        }
    }
}