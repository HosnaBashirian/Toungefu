using UnityEngine;
using TMPro;

public class TriggerTextPrompt : MonoBehaviour
{
   
    public GameObject TriggerCube1;
    public GameObject TriggerCube2;
    public GameObject TriggerCube3;
    public GameObject TriggerCube4;
    
    public string promptText1 = "prompt 1";
    public string promptText2 = "prompt 2";
    public string promptText3 = "prompt 3";
    public string promptText4 = "prompt 4";

    public GameObject textPromptUI; 
    public float displayDuration = 3f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered a trigger");

            // check which trigger was entered
            if (gameObject == TriggerCube1)
            {
                Debug.Log("TriggerCube1 activated");
                ShowTextPrompt(promptText1);
            }
            else if (gameObject == TriggerCube2)
            {
                Debug.Log("TriggerCube2 activated");
                ShowTextPrompt(promptText2);
            }
            else if (gameObject == TriggerCube3)
            {
                Debug.Log("TriggerCube3 activated");
                ShowTextPrompt(promptText3);
            }
            else if (gameObject == TriggerCube4)
            {
                Debug.Log("TriggerCube4 activated");
                ShowTextPrompt(promptText4);
            }
            else
            {
                // Debug.Log("player entered a trigger, but it's not any of the triggers you set");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("player exited a trigger");
            HideTextPrompt();
        }
    }

    private void ShowTextPrompt(string text)
    {
        if (textPromptUI != null)
        {
            TextMeshProUGUI tmpComponent = textPromptUI.GetComponent<TextMeshProUGUI>();
            if (tmpComponent != null)
            {
                tmpComponent.text = text;
                textPromptUI.SetActive(true);
                Invoke("HideTextPrompt", displayDuration);
            }
            else
            {
                Debug.LogError("component not found!");
            }
        }
    }

    private void HideTextPrompt()
    {
        if (textPromptUI != null)
        {
            textPromptUI.SetActive(false);
        }
    }
}