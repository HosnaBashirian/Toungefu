using UnityEngine;
using TMPro;

public class RiddleTextPrompt : MonoBehaviour
{
    public GameObject TriggerCube1;
    public GameObject TriggerCube2;
    public GameObject TriggerCube3;

    public string promptRiddle1 = "riddle 1";
    public string promptRiddle2 = "riddle 2";
    public string promptRiddle3 = "riddle 3";

    public GameObject textPromptUI;
    public float displayDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered a trigger");

            // check which trigger was entered
            if (gameObject == TriggerCube1)
            {
                Debug.Log("TriggerCube1 activated");
                ShowTextPrompt(promptRiddle1);
            }
            else if (gameObject == TriggerCube2)
            {
                Debug.Log("TriggerCube2 activated");
                ShowTextPrompt(promptRiddle2);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
