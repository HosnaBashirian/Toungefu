using UnityEngine;
using TMPro;

public class DoorLickPuzzle : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    private bool messageShown = false;

    public string[] correctOrder = { "Red", "Yellow", "Green", "Blue" };
    private int currentOrderIndex = 0;

    private void Update()
    {
        // Check if the puzzle is solved and display a message
        if (currentOrderIndex == correctOrder.Length && !messageShown)
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
        }
        
        Destroy(gameObject);
    }

    public void LickLollipop(string lollipopColor)
    {
        // Check if the licked lollipop is the next in the correct order
        if (lollipopColor == correctOrder[currentOrderIndex])
        {
            currentOrderIndex++;
            // Debug.Log("Correct! Licked " + lollipopColor + ". Progress: " + currentOrderIndex + "/" + correctOrder.Length);
            
            if (currentOrderIndex == correctOrder.Length)
            {
                // Debug.Log("All lollipops licked in the correct order! Door unlocked!");
            }
        }
        else
        {
            currentOrderIndex = 0;
            ShowMessage("Wrong order! Try again");
        }
    }
}